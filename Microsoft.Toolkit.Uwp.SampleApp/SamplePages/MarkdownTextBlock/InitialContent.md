This control was originally written by [Quinn Damerell](https://github.com/QuinnDamerell) and [Paul Bartrum](https://github.com/paulbartrum) for [Baconit](https://github.com/QuinnDamerell/Baconit), a popular open source reddit UWP. The control *almost* supports the full markdown syntax, with a focus on super-efficient parsing and rendering. The control is efficient enough to be used in virtualizing lists.

&nbsp;

*Note:* For a full list of markdown syntax, see the [official syntax guide](http://daringfireball.net/projects/markdown/syntax).

&nbsp;

**Try it live!** Type in the *unformatted text box* above!

&nbsp;

# PARAGRAPHS

Paragraphs are delimited by a blank line.  Simply starting text on a new line won't create a new paragraph; It will remain on the same line in the final, rendered version as the previous line.  You need an extra, blank line to start a new paragraph.  This is especially important when dealing with quotes and, to a lesser degree, lists.

You can also add non-paragraph line breaks by ending a line with two spaces.  The difference is subtle:

Paragraph 1, Line 1  
Paragraph 1, Line 2  

Paragraph 2

*****

# FONT FORMATTING
### Italics

Text can be displayed in an italic font by surrounding a word or words with either single asterisks (\*) or single underscores (\_).

For example: 

>This sentence includes \*italic text\*.

is displayed as:

>This sentence includes *italic text*.


### Bold

Text can be displayed in a bold font by surrounding a word or words with either double asterisks (\*) or double underscores (\_).

For example: 

>This sentence includes \*\*bold text\*\*.

is displayed as:

>This sentence includes **bold text**.

### Strikethrough

Text can be displayed in a strikethrough font by surrounding a word or words with double tildes (~~).  For example:

>This sentence includes ~ ~strikethrough text~ ~ 

>*(but with no spaces between the tildes; escape sequences [see far below] appear not to work with tildes, so I can't demonstrate the exact usage).*

is displayed as:

>This sentence includes ~~strikethrough text~~.

### Superscript

Text can be displayed in a superscript font by preceding it with a caret ( ^ ).  

>This sentence includes super^ script 

>*(but with no spaces after the caret; Like strikethrough, the superscript syntax doesn't play nicely with escape sequences).*

is displayed as:

>This sentence includes super^script.

Superscripts can even be nested: just^like^this .

However, note that the superscript font will be reset by a space.  To get around this, you can enclose the text in the superscript with parentheses.  The parentheses won't be displayed in the comment, and everything inside of them will be superscripted, regardless of spaces:

>This sentence^ (has a superscript with multiple words)

>*Once again, with no space after the caret.*

is displayed as

>This sentence^(has a superscript with multiple words)

### Headers

Markdown supports 6 levels of headers (some of which don't actually display as headers in reddit):

#Header 1

##Header 2

###Header 3

####Header 4

#####Header 5

######Header 6

...which can be created in a couple of different ways.  Level 1 and 2 headers can be created by adding a line of equals signs (=) or dashes (\-), respectively, underneath the header text. 

However, *all* types of headers can be created with a second method.  Simply prepend a number of hashes (#) corresponding to the header level you want, so:

>\# Header 1

>\#\# Header 2

>\#\#\# Header 3

>\#\#\#\# Header 4

>\#\#\#\#\# Header 5

>\#\#\#\#\#\# Header 6

results in:

>#Header 1

>##Header 2

>###Header 3

>####Header 4

>#####Header 5

>######Header 6

Note: you can add hashes after the header text to balance out how the source code looks without affecting what is displayed. So:

>\#\# Header 2 ##

also produces:

>## Header 2


*****
# LISTS

Markdown supports two types of lists: ordered and unordered.

### Unordered Lists

Prepend each element in the list with either a plus (+), dash (-), or asterisk (*) plus a space.  Line openers can be mixed.  So

>\* Item 1

>\+ Item 2

>\- Item 3

results in

>* Item 1
>+ Item 2
>- Item 3
 


### Ordered Lists

Ordered lists work roughly the same way, but you prepend each item in the list with a number plus a period (.) plus a space.  Also, it makes no difference what numbers you use.  The ordered list will always start with the number 1, and will always increment sequentially.  So

>7\. Item 1

>2\. Item 2

>5\. Item 3

results in

>7. Item 1
>2. Item 2
>5. Item 3

Also, you can nest lists, like so:

1. Ordered list item 1

2. * Bullet 1 in list item 2
    * Bullet 2 in list item 2

3. List item 3

Note: If your list items consist of multiple paragraphs, you can force each new paragraph to remain in the previous list item by indenting it by one tab or four spaces.  So

>\* This item has multiple paragraphs.
>
>(*four spaces here*)This is the second paragraph
>
>\* Item 2

results in:

>* This item has multiple paragraphs.
>
>    This is the second paragraph
>* Item 2

Notice how the spaces in my source were stripped out?  What if you need to preserve formatting?  That brings us to:

*****

# CODE BLOCKS AND INLINE CODE

Inline code is easy.  Simply surround any text with backticks (\`), **not to be confused with apostrophes (')**.  Anything between the backticks will be rendered in a fixed-width font, and none of the formatting syntax we're exploring will be applied.  So

>Here is some `` ` ``inline code with \*\*formatting\*\*`` ` ``

is displayed as:

>Here is some `inline code with **formatting**`

Note that this is why you should use the normal apostrophe when typing out possessive nouns or contractions.  Otherwise you may end up with something like:

>I couldn`t believe that he didn`t know that!

Sometimes you need to preserve indentation, too.  In those cases, you can create a block code element by starting every line of your code with four spaces (followed by other spaces that will be preserved).  You can get results like the following:

    public void main(Strings argv[]){
        System.out.println("Hello world!");
    }

Starting with UWP Community Toolkit v1.4, you can also use GitHub code notification by creating a block surrounded by 3x\` (3 backticks). So:

\`\`\`

public void main(Strings argv[]){

  System.out.println("Hello world!");

}

\`\`\`

will produce:

```
public void main(Strings argv[]){
    System.out.println("Hello world!");
}
```

*****

# LINKS

There are a couple of ways to get HTML links.  The easiest is to just paste a valid URL, which will be automatically parsed as a link.  Like so:

>http://en.wikipedia.org

However, usually you'll want to have text that functions as a link.  In that case, include the text inside of square brackets followed by the URL in parentheses. So:

>\[Wikipedia\]\(http://en.wikipedia.org).

results in:

>[Wikipedia](http://en.wikipedia.org).

You can also provide tooltip text for links like so:

>\[Wikipedia\]\(http://en.wikipedia.org "tooltip text"\).

results in:

>[Wikipedia](http://en.wikipedia.org "tooltip text").

There are other methods of generating links that aren't appropriate for discussion-board style comments.  See the [Markdown Syntax](http://daringfireball.net/projects/markdown/syntax#link) if you're interested in more info.

*****

# IMAGES

To add an image, it is almost like a link. You just need to add a \! before.

So inline image syntax looks like this:

>\!\[Toolkit logo](https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/master/Microsoft.Toolkit.Uwp.SampleApp/Assets/ToolkitLogo.png)

which renders in:

![Toolkit logo](https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/master/Microsoft.Toolkit.Uwp.SampleApp/Assets/ToolkitLogo.png)

*****

# BLOCK QUOTES

You'll probably do a lot of quoting of other redditors.  In those cases, you'll want to use block quotes.  Simple begin each line you want quoted with a right angle bracket (>).  Multiple angle brackets can be used for nested quotes.  To cause a new paragraph to be quoted, begin that paragraph with another angle bracket.  So the following:

    >Here's a quote.
    
    >Another paragraph in the same quote.
    >>A nested quote.

    >Back to a single quote.

    And finally some unquoted text.


Is displayed as:


>Here's a quote.
    
>Another paragraph in the same quote.
>>A nested quote.

>Back to a single quote.

And finally some unquoted text.

*****

# MISCELLANEOUS

### Tables

Reddit has the ability to represent tabular data in fancy-looking tables.  For example:

some|header|labels
:---|:--:|---:
Left-justified|center-justified|right-justified
a|b|c
d|e|f

Which is produced with the following markdown:

>`some|header|labels`  
>`:---|:--:|---:`  
>`Left-justified|center-justified|right-justified`  
>`a|b|c`  
>`d|e|f`

All you need to produce a table is a row of headers separated by "pipes" (**|**), a row indicating how to justify the columns, and 1 or more rows of data (again, pipe-separated).

The only real "magic" is in the row between the headers and the data.  It should ideally be formed with rows dashes separated by pipes.  If you add a colon to the left of the dashes for a column, that column will be left-justified.  To the right for right justification, and on both sides for centered data.  If there's no colon, it defaults to left-justified.

Any number of dashes will do, even just one.  You can use none at all if you want it to default to left-justified, but it's just easier to see what you're doing if you put a few in there.

Also note that the pipes (signifying the dividing line between cells) don't have to line up.  You just need the same number of them in every row.

### Escaping special characters

If you need to display any of the special characters, you can escape that character with a backslash (\\).  For example:

>Escaped \\\*italics\\\*

results in:

>Escaped \*italics\*

###Horizontal rules

Finally, to create a horizontal rule, create a separate paragraph with 5 or more asterisks (\*).

>\*\*\*\*\*

results in

>*****

Source: https://www.reddit.com/r/reddit.com/comments/6ewgt/reddit_markdown_primer_or_how_do_you_do_all_that/c03nik6
