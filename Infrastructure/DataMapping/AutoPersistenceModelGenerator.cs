using System;
using DomainLayer.Base;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using Infrastructure.NHibernateMaps.Conventions;
using SharpArch.Domain.DomainModel;
using SharpArch.NHibernate.FluentNHibernate;

namespace Infrastructure.DataMapping
{
    /// <summary>
    /// Generates the automapping for the domain assembly
    /// </summary>
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate()
        {
            var mappings = AutoMap.AssemblyOf<BaseModel>(new AutomappingConfiguration());
            mappings.IgnoreBase<Entity>();
            mappings.IgnoreBase(typeof(EntityWithTypedId<>));
            mappings.Conventions.Setup(GetConventions());
            mappings.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }

        private static Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
               // c.Add<PrimaryKeyConvention>();
                c.Add<CustomForeignKeyConvention>();
                c.Add<HasManyConvention>();
                c.Add<TableNameConvention>();
                c.Add<EnumConvention>();
                c.Add<StringColumnLengthConvention>();
                c.Add<BinaryColumnLengthConvention>();
                c.Add<PrimaryKeyConvention>();
            };
        }
    }
}
