using System.Linq;
using HotChocolate;
using ConferencePlanner.GraphQL.Data;
using Microsoft.EntityFrameworkCore;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Types.Relay;
using HotChocolate.Types;

namespace ConferencePlanner.GraphQL;

public class SpeakerQueries
{
[UseApplicationDbContext]
[UsePaging]
public IQueryable<Speaker> GetSpeakers(
    [ScopedService] ApplicationDbContext context) =>
    context.Speakers.OrderBy(t => t.Name);

    public Task<Speaker> GetSpeakerByIdAsync(
             [ID(nameof(Speaker))] int id,
             SpeakerByIdDataLoader dataLoader,
             CancellationToken cancellationToken) =>
    dataLoader.LoadAsync(id, cancellationToken);
    public async Task<IEnumerable<Speaker>> GetSpeakersByIdAsync(
        [ID(nameof(Speaker))] int[] ids,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken) =>
        await dataLoader.LoadAsync(ids, cancellationToken);
}


