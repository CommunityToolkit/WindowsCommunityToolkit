using AppStudio.DataProviders.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders
{
    public abstract class DataProviderBase<TConfig>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            ValidateConfig(config);

            var result = await GetDataAsync(config, maxRecords, parser);
            if (result != null)
            {
                return result
                    .Take(maxRecords)
                    .ToList();             
            }
            return new TSchema[0];
        }

        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract void ValidateConfig(TConfig config);
    }

    public abstract class DataProviderBase<TConfig, TSchema> : DataProviderBase<TConfig> where TSchema : SchemaBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20)
        {
            return await LoadDataAsync(config, maxRecords, GetDefaultParser(config));
        }

        public IParser<TSchema> GetDefaultParser(TConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            ValidateConfig(config);

            return GetDefaultParserInternal(config);
        }
        protected abstract IParser<TSchema> GetDefaultParserInternal(TConfig config);
    }
}
