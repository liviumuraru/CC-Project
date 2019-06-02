using Accelerant.DataTransfer.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Accelerant.DataLayer.DataProviders.Mongo
{
    public class WorkspaceProvider
        : AMongoDbProvider<Workspace>
    {
        public WorkspaceProvider(IMongoCollection<Workspace> workspaceCollection)
            : base(workspaceCollection)
        {
        }

        public override Workspace Get(Guid Id)
        {
            return dataCollection.Find(x => x.Id == Id).First();
        }

        public override IEnumerable<Workspace> GetAll()
        {
            return dataCollection.AsQueryable().ToList();
        }

        public override IEnumerable<Workspace> GetAllForUser(Guid Id)
        {
            return dataCollection.AsQueryable().Where(x => x.Id == Id).ToList();
        }

        public override IEnumerable<Workspace> GetMany(IEnumerable<Guid> Id)
        {
            throw new NotImplementedException();
        }
    }
}
