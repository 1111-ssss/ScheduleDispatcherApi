using Infrastructure.Tests.Fixtures;

namespace Infrastructure.Tests.Collections;

[CollectionDefinition(nameof(DatabaseTestCollection))]
public class DatabaseTestCollection : ICollectionFixture<InMemoryDbContextFixture>
{
    
}
