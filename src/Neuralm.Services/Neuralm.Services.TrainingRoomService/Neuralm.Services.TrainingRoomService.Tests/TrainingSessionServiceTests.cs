using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore.Repositories;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Mapping;
using Neuralm.Services.TrainingRoomService.Messages;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using Neuralm.Services.TrainingRoomService.Persistence.Infrastructure;
using Neuralm.Services.TrainingRoomService.Persistence.Repositories;
using Neuralm.Services.TrainingRoomService.Persistence.Validators;
using Neuralm.Services.TrainingRoomService.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Neuralm.Services.TrainingRoomService.Tests
{
    [TestClass]
    public class TrainingSessionServiceTests
    {
        public ILogger<TrainingSessionServiceTests> Logger { get; set; }
        public ITrainingSessionService TrainingSessionService { get; set; }
        public IGenericServiceProvider GenericServiceProvider { get; set; }
        public SqliteConnection SqliteConnection { get; set; }
        public Guid TrainingRoomId { get; set; }
        public Guid UserId { get; set; }
        public int OrganismCount { get; set; } = 15;

        [TestInitialize]
        public async Task Initialize()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddConsole());
            serviceCollection.AddAutoMapper(Assembly.GetAssembly(typeof(TrainingRoomStartupExtensions)));

            #region Database
            SqliteConnection = new SqliteConnection("Data Source=:memory:");
            SqliteConnection.Open();

            serviceCollection.AddDbContext<TrainingRoomDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlite(SqliteConnection);
                optionsBuilder.EnableSensitiveDataLogging();
            });
            #endregion Database

            #region Validators
            serviceCollection.AddTransient<IEntityValidator<TrainingRoom>, TrainingRoomValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingSession>, TrainingSessionValidator>();
            serviceCollection.AddTransient<IEntityValidator<TrainingRoomSettings>, TrainingRoomSettingsValidator>();
            #endregion Validators

            #region Repositories
            serviceCollection.AddTransient<IRepository<TrainingRoom>, Repository<TrainingRoom, TrainingRoomDbContext>>();
            serviceCollection.AddTransient<ITrainingSessionRepository, TrainingSessionRepository>();
            serviceCollection.AddTransient<IRepository<TrainingRoomSettings>, Repository<TrainingRoomSettings, TrainingRoomDbContext>>();
            #endregion Repositories

            #region Services
            serviceCollection.AddTransient<ITrainingRoomService, Application.Services.TrainingRoomService>();
            serviceCollection.AddTransient<ITrainingSessionService, Application.Services.TrainingSessionService>();
            UserId = Guid.NewGuid();
            const string username = "Mario";
            serviceCollection.AddSingleton<IUserService, UserServiceMock>(p => new UserServiceMock(UserId, username));
            #endregion Services

            GenericServiceProvider = serviceCollection.BuildServiceProvider().ToGenericServiceProvider();
            Logger = GenericServiceProvider.GetService<ILogger<TrainingSessionServiceTests>>();
            Logger.LogInformation("Started initialization!");
            TrainingSessionService = GenericServiceProvider.GetService<ITrainingSessionService>();

            TrainingRoomDbContext context = GenericServiceProvider.GetService<TrainingRoomDbContext>();
            await context.Database.EnsureCreatedAsync();

            ITrainingRoomService trainingRoomService = GenericServiceProvider.GetService<ITrainingRoomService>();
            IUserService userService = GenericServiceProvider.GetService<IUserService>();

            TrainingRoomDto trainingRoomDto = new TrainingRoomDto()
            {
                Id = Guid.NewGuid(),
                Name = "Cool room",
                OwnerId = UserId,
                Owner = await userService.FindUserAsync(UserId),
                Generation = 0,
                TrainingRoomSettings = new TrainingRoomSettingsDto()
                {
                    Id = Guid.NewGuid(),
                    OrganismCount = (uint)OrganismCount,
                    InputCount = 2,
                    OutputCount = 1,
                    SpeciesExcessGeneWeight = 1,
                    SpeciesDisjointGeneWeight = 1,
                    SpeciesAverageWeightDiffWeight = 0.4,
                    Threshold = 3,
                    AddConnectionChance = 0.05,
                    AddNodeChance = 0.03,
                    CrossOverChance = 0.75,
                    InterSpeciesChance = 0.001,
                    MutationChance = 1,
                    MutateWeightChance = 0.8,
                    WeightReassignChance = 0.1,
                    TopAmountToSurvive = 0.5,
                    EnableConnectionChance = 0.25,
                    Seed = 1
                },
                AuthorizedTrainers = new List<TrainerDto>(),
                TrainingSessions = new List<TrainingSessionDto>()
            };
            (bool success, Guid id) = await trainingRoomService.CreateAsync(trainingRoomDto);
            
            Logger.LogInformation($"Created a training room: {success} id: {id}");
            TrainingRoomId = id;
            Logger.LogInformation("Finished initialization!");
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            Logger.LogInformation("Started cleanup!");
            TrainingRoomDbContext context = GenericServiceProvider.GetService<TrainingRoomDbContext>();
            await context.Database.EnsureDeletedAsync();
            await GenericServiceProvider.DisposeAsync();
            Logger.LogInformation("Finished cleanup!");
        }

        [TestMethod]
        public async Task StartTrainingSessionAsync_Should_Create_A_New_Session()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = TrainingRoomId, 
                UserId = UserId
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Assert.IsTrue(startTrainingSessionResponse.Success);
        }

        [TestMethod]
        public async Task StartTrainingSessionAsync_Should_Fail_Due_To_Unknown_User_Id()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = TrainingRoomId,
                UserId = Guid.NewGuid()
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Assert.IsFalse(startTrainingSessionResponse.Success);
            Assert.AreEqual("User does not exist.", startTrainingSessionResponse.Message);
        }

        [TestMethod]
        public async Task StartTrainingSessionAsync_Should_Fail_Due_To_Unknown_TrainingRoom_Id()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = Guid.NewGuid(),
                UserId = UserId
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Assert.IsFalse(startTrainingSessionResponse.Success);
            Assert.AreEqual("Training room does not exist.", startTrainingSessionResponse.Message);
        }

        [TestMethod]
        public async Task EndTrainingSessionAsync_Should_Successfully_End_The_TrainingSession()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = TrainingRoomId,
                UserId = UserId
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Guid trainingSessionId = startTrainingSessionResponse.TrainingSession.Id;

            EndTrainingSessionRequest endTrainingSessionRequest = new EndTrainingSessionRequest()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingSessionId = trainingSessionId
            };
            EndTrainingSessionResponse endTrainingSessionResponse = await TrainingSessionService.EndTrainingSessionAsync(endTrainingSessionRequest);
            Assert.IsTrue(endTrainingSessionResponse.Success);
        }

        [TestMethod]
        public async Task EndTrainingSessionAsync_Should_Fail_Due_To_Unknown_TrainingSession_Id()
        {
            EndTrainingSessionRequest endTrainingSessionRequest = new EndTrainingSessionRequest()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingSessionId = Guid.NewGuid()
            };
            EndTrainingSessionResponse endTrainingSessionResponse = await TrainingSessionService.EndTrainingSessionAsync(endTrainingSessionRequest);
            Assert.IsFalse(endTrainingSessionResponse.Success);
            Assert.AreEqual("Training session does not exist.", endTrainingSessionResponse.Message);
        }

        [TestMethod]
        public async Task GetOrganismsAsync_Should_Successfully_Return_15_Organisms()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = TrainingRoomId,
                UserId = UserId
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Guid trainingSessionId = startTrainingSessionResponse.TrainingSession.Id;

            GetOrganismsRequest getOrganismsRequest = new GetOrganismsRequest()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingSessionId = trainingSessionId,
                Amount = OrganismCount,
                UserId = UserId
            };
            GetOrganismsResponse getOrganismsResponse = await TrainingSessionService.GetOrganismsAsync(getOrganismsRequest);
            Assert.IsTrue(getOrganismsResponse.Success);
            Assert.AreEqual(OrganismCount, getOrganismsResponse.Organisms.Count);
        }

        [TestMethod]
        public async Task PostOrganismsAsync_Should_Successfully_Start_A_New_Generation()
        {
            StartTrainingSessionRequest startTrainingSessionRequest = new StartTrainingSessionRequest
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingRoomId = TrainingRoomId,
                UserId = UserId
            };
            StartTrainingSessionResponse startTrainingSessionResponse = await TrainingSessionService.StartTrainingSessionAsync(startTrainingSessionRequest);
            Guid trainingSessionId = startTrainingSessionResponse.TrainingSession.Id;

            GetOrganismsRequest getOrganismsRequest = new GetOrganismsRequest()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingSessionId = trainingSessionId,
                Amount = OrganismCount,
                UserId = UserId
            };
            GetOrganismsResponse getOrganismsResponse = await TrainingSessionService.GetOrganismsAsync(getOrganismsRequest);

            List<DictionaryEntry<Guid, double>> scores = new List<DictionaryEntry<Guid, double>>();
            foreach (OrganismDto organism in getOrganismsResponse.Organisms)
            {
                scores.Add(new DictionaryEntry<Guid, double>() { Key = organism.Id, Value = organism.Score + 2 });
            }

            PostOrganismsScoreRequest postOrganismsScoreRequest = new PostOrganismsScoreRequest()
            {
                Id = Guid.NewGuid(),
                DateTime = DateTime.Now,
                TrainingSessionId = trainingSessionId,
                OrganismScores = scores
            };
            PostOrganismsScoreResponse postOrganismsScoreResponse = await TrainingSessionService.PostOrganismsScoreAsync(postOrganismsScoreRequest);
            Assert.IsTrue(postOrganismsScoreResponse.Success);
            Assert.AreEqual("Successfully updated the organisms and advanced a generation!", postOrganismsScoreResponse.Message);
        }
    }
}
