// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

// Recognizes command line argument keywords such as "-help" or "-include". Matches partial
// strings as long as they uniquely identify the keyword. Matching is case insenstive.
sealed class CommandlineTokenizer<TKeywordId> where TKeywordId : struct, IComparable
{
    readonly TrieNode m_root = new TrieNode('\0');
    readonly TKeywordId m_ambiguousValue;

    internal CommandlineTokenizer(TKeywordId ambiguousValue)
    {
        m_ambiguousValue = ambiguousValue;
    }

    internal CommandlineTokenizer<TKeywordId> AddPrefixedKeyword(string keyword, TKeywordId id)
    {
        AddKeyword($"-{keyword}", id);
        return AddKeyword($"/{keyword}", id);
    }

    // Add a keyword to the recognizer.
    internal CommandlineTokenizer<TKeywordId> AddKeyword(string keyword, TKeywordId id)
    {
        TrieNode currrentNode = m_root;

        foreach (char ch in keyword)
        {
            // Search through the current set of nodes to see if there is a node
            // for the current character already. If there is, use it, if not, add one.
            bool matched = false;
            foreach (TrieNode n in currrentNode.Children)
            {
                if (n.Matches(ch))
                {
                    matched = true;
                    currrentNode = n;
                    // Reusing an existing node. If has an id that is not the same as our
                    // id, the string is not unique, so remove it to indicate there is
                    // no match to this point. If the node is a terminal (i.e. it has
                    // no children) then this keyword cannot be added as it either
                    // includes an existing keyword, or is the same as an existing keyword.
                    if (n.Children.Count == 0)
                    {
                        if (n.Keyword.Value.CompareTo(id) == 0)
                        {
                            throw new InvalidOperationException("Equivalent keyword already added.");
                        }
                        else
                        {
                            throw new InvalidOperationException("Keyword redefinition.");
                        }
                    }
                    else if (n.Keyword.HasValue && n.Keyword.Value.CompareTo(id) != 0)
                    {
                        n.Keyword = null;
                    }
                    break;
                }
            }

            // If no match found, add a new node
            if (!matched)
            {
                var newNode = new TrieNode(ch) { Keyword = id };
                currrentNode.Children.Add(newNode);
                currrentNode = newNode;
            }
        }
        if (currrentNode.Keyword == null)
        {
            throw new InvalidOperationException("Keyword matches an existing prefix.");
        }
        return this;
    }

    internal IEnumerable<(TKeywordId, string)> Tokenize(string[] args)
    {
        foreach (var arg in args)
        {
            TryMatchKeyword(arg, out var keywordId);
            yield return (keywordId, arg);
        }
    }

    // Attempts to match the keyword to one that has been added to the recognizer. On success
    // return true and sets the id to the matching keyword. On failure returns false
    // and sets the id to default if no part of the keyword matched or to the ambigous value
    // if the match was ambiguous.
    internal bool TryMatchKeyword(string keyword, out TKeywordId id)
    {
        TrieNode currentNode = m_root;

        if (keyword != null)
        {
            foreach (char ch in keyword)
            {
                bool matched = false;
                foreach (TrieNode n in currentNode.Children)
                {
                    if (n.Matches(ch))
                    {
                        matched = true;
                        currentNode = n;
                        break;
                    }
                }

                // If no match found, return false.
                if (!matched)
                {
                    id = default;
                    return false;
                }
            }
        }

        id = currentNode.Keyword.HasValue ? currentNode.Keyword.Value : (currentNode == m_root ? default : m_ambiguousValue);
        return currentNode.Keyword != null;
    }

    sealed class TrieNode
    {
        readonly char m_character;
        internal TKeywordId? Keyword;

        internal TrieNode(char character)
        {
            m_character = char.ToLower(character);
        }

        internal List<TrieNode> Children { get; } = new List<TrieNode>();

        internal bool Matches(char character) =>  char.ToLower(character) == m_character;
    }
}
