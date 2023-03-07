﻿using FluentAssertions;
using NScenario;
using ProfileService.FunctionalTests.Adapters;
using ProfileService.GRPC;
using Xunit.Abstractions;
using static ProfileService.GRPC.ProfileService;
using DiscountType = ProfileService.GRPC.DiscountType;
using static ProfileService.TestDataGeneratorsAndExtensions.DataGenerator;

namespace ProfileService.FunctionalTests.Scenaries.Discount
{
    public class ScenarioUpdateDiscountTests : IClassFixture<TestServerFixture>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly ProfileServiceClient _client;

        public ScenarioUpdateDiscountTests(TestServerFixture factory,
            ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _client = new ProfileServiceClient(factory.GrpcChannel);
        }

        [Fact()]
        public async Task ScenarioUpdateDiscount()
        {

            string personalId = Guid.NewGuid().ToString();
            string discountId = Guid.NewGuid().ToString();

            var personalProfile = PersonalProfileGenerator(personalId);
            var discount = DiscountGenerator(discountId, personalId, DiscountType.Amount, 50);
            var discount2 = DiscountGenerator(discountId, personalId, DiscountType.Amount, 50);
            discount2.Isalreadyused = true;

            var scenario = TestScenarioFactory.Default(
                new XUnitOutputAdapter(_outputHelper),
                testMethodName: $"UpdateDiscount");

            var addPersonalDataResponse = await scenario
                .Step($"Add PersonalData",
                    async () =>
                    {
                        var request = new AddPersonalDataRequest()
                        {
                            Personalprofile = personalProfile
                        };

                        return await _client.AddPersonalDataAsync(request);
                    });

            var addDiscountResponse = await scenario
                .Step($"Add Discount",
                    async () =>
                    {
                        var request = new AddDiscountRequest()
                        {
                            Discount = discount
                        };

                        return await _client.AddDiscountAsync(request);
                    });

            var updateDiscountResponse = await scenario
                .Step($"Update Discount",
                async () =>
                {
                    var request = new UpdateDiscountRequest()
                    {
                        Discount = discount2
                    };

                    return await _client.UpdateDiscountAsync(request);
                });

            var getDiscountsResponse = await scenario
                .Step($"Get Discounts",
                async () =>
                {
                    var request = new GetDiscountsRequest()
                    {
                        Profilebyidrequest = new ProfileByIdRequest()
                        {
                            Id = personalId,
                        }
                    };
                    return await _client.GetDiscountsAsync(request);
                });

            var result = getDiscountsResponse.Discounts.First();

            result
                .Should()
                .NotBeNull()
                .Equals(discount2);
        }
    }
}
