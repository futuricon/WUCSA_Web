using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class RankRepository : Repository, IRankRepository
    {
        public readonly ApplicationDbContext _context;

        public RankRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //////////////// Rank ////////////////

        public Task AddRankAsync(Rank rank)
        {
            rank.Slug = GetVerifiedRankSlug(rank);
            return AddAsync(rank);
        }

        public Task UpdateRankAsync(Rank rank)
        {
            rank.Slug = GetVerifiedRankSlug(rank);
            return UpdateAsync(rank);
        }

        public async Task DeleteRankAsync(Rank rank)
        {
            foreach (var rankParticipant in rank.RankParticipants)
            {
                await DeleteRankParticipantAsync(rankParticipant, false);
            }

            await DeleteAsync(rank);
        }

        private string GetVerifiedRankSlug(Rank slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Rank>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Rank>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }


        //////////////// RankParticipant ////////////////

        public Task AddRankParticipantAsync(RankParticipant rankParticipant)
        {
            return AddAsync(rankParticipant);
        }

        public Task UpdateRankParticipantAsync(RankParticipant rankParticipant)
        {
            return UpdateAsync(rankParticipant);
        }

        public async Task DeleteRankParticipantAsync(RankParticipant rankParticipant, bool saveChanges = true)
        {
            foreach (var participant in rankParticipant.Participants)
            {
                await DeleteAsync(participant);
            }

            await DeleteAsync(rankParticipant);

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }



        //////////////// Participant ////////////////

        public async Task UpdateParticipantAsync(RankParticipant rankParticipant, 
            bool saveChanges = true, params Participant[] newParts)
        {
            var existingParts = (await GetByIdAsync<RankParticipant>(rankParticipant.Id)).Participants.ToList();
            List<Participant> toUpdateParts = new List<Participant>();
            if (existingParts != null)
            {
                foreach (var existingPart in existingParts)
                {
                    if (!newParts.Any(i => i.Id == existingPart.Id))
                    {
                        await DeleteAsync(existingPart);
                    }
                    toUpdateParts.Add(newParts.Where(i => i.Id == existingPart.Id).FirstOrDefault());
                    newParts.ToList().Remove(newParts.Where(note => note.Id == existingPart.Id).FirstOrDefault());
                }
            }
            foreach (var newPart in newParts)
            {
                newPart.RankParticipant = rankParticipant;
                await AddAsync(newPart);
            }
            if (toUpdateParts != null)
            {
                foreach (var toUpdatePart in toUpdateParts)
                {
                    await UpdateAsync(toUpdatePart);
                }
            }

            if (saveChanges)
            {
                await UpdateAsync(rankParticipant);
            }
        }



        //////////////// SportType ////////////////

        public Task AddSportTypeAsync(SportType sportType)
        {
            sportType.Slug = GetVerifiedSportTypeSlug(sportType);
            return AddAsync(sportType);
        }

        public Task UpdateSportTypeAsync(SportType sportType)
        {
            sportType.Slug = GetVerifiedSportTypeSlug(sportType);
            return UpdateAsync(sportType);
        }

        public async Task DeleteSportTypeAsync(SportType sportType)
        {
            await DeleteAsync(sportType);
        }

        private string GetVerifiedSportTypeSlug(SportType slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<SportType>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<SportType>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }
    }
}
