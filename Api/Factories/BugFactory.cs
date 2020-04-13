using BugDbEntity = Api.Database.Models.Bug;
using BugModel = Api.Models.Bug;
using AppDbEntity = Api.Database.Models.App;
using AppModel = Api.Models.App;

namespace Api.Models.Factories {
    public static class BugFactory {
        public static BugDbEntity Create(BugModel model) {
            if (model == default) return default;
            return new BugDbEntity(model.Id, model.Summary, model.Description, model.Status, model.ReportDate,
                model.Reporter, model.App.Id);
        }

        public static BugModel Create(BugDbEntity entity) {
            if (entity == default) return default;
            return new BugModel(entity.Id, entity.Summary, entity.Description, entity.Reporter, entity.App.Version,
                entity.Status, entity.ReportDate);
        }
    }
}