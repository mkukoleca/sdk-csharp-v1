﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Xunit;
using System.Text;
using System.Threading.Tasks;
using Trulioo.Client.V1.Model.BusinessSearch;
using static Trulioo.Client.V1.Model.BusinessSearch.BusinessSearchRequest;
using System.Linq;

namespace Trulioo.Client.V1.Tests
{
    public class BusinessFacts
    {
        private readonly string _username = ConfigurationManager.AppSettings["kyb_username"];
        private readonly string _password = ConfigurationManager.AppSettings["kyb_password"];
        private readonly string _hostEndpoint = ConfigurationManager.AppSettings["host"];

        [Theory(Skip = "Calls API")]
        [MemberData(nameof(BusinessSearchTestData))]
        public async Task BusinessSearchTest(BusinessSearchRequest request, BusinessSearchResponse expectedResponse)
        {
            using (var client = new TruliooApiClient(new Context(_username, _password) { Host = _hostEndpoint }))
            {
                var response = await client.Business.BusinessSearchAsync(request);
                
                Assert.Equal(expectedResponse.Record.RecordStatus, response.Record.RecordStatus);
                Assert.Equal(expectedResponse.CountryCode, response.CountryCode);

                Assert.Equal(expectedResponse.Record.DatasourceResults.Count(), response.Record.DatasourceResults.Count());
                List<string> expectedDatasourcesNames = expectedResponse.Record.DatasourceResults.Select(x => x.DatasourceName).ToList();
                List<string> actualDatasourceNames = response.Record.DatasourceResults.Select(x => x.DatasourceName).ToList();
                Assert.True(expectedDatasourcesNames.All(actualDatasourceNames.Contains));

                List<string> expectedBusinessNameResults = expectedResponse.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessName)).ToList();
                List<string> actualBusinessNameResults = response.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessName)).ToList();
                Assert.Equal(expectedBusinessNameResults.Count(), actualBusinessNameResults.Count());
                Assert.True(expectedBusinessNameResults.All(actualBusinessNameResults.Contains));

                List<string> expectedBusinessNumberResults = expectedResponse.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessRegistrationNumber)).ToList();
                List<string> actualBusinessNumberResults = response.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessRegistrationNumber)).ToList();
                Assert.Equal(expectedBusinessNumberResults.Count(), actualBusinessNumberResults.Count());
                Assert.True(expectedBusinessNumberResults.All(actualBusinessNumberResults.Contains));
            }
        }

        public static IEnumerable<object[]> BusinessSearchTestData()
        {
            yield return new object[] {
                new BusinessSearchRequest() {
                    AcceptTruliooTermsAndConditions = true,
                    CountryCode = "",
                    Business = new BusinessSearchModel
                    {
                        BusinessName = "",
                        JurisdictionOfIncorporation = ""
                    }
                },
                new BusinessSearchResponse()
                {
                    TransactionID = "",
                    CountryCode = "",
                    Record = new BusinessRecord()
                    {
                        RecordStatus = "",
                        DatasourceResults = new List<BusinessSearchResult>{ new BusinessSearchResult {
                            DatasourceName = "",
                            Results = new List<SearchResult>
                            {
                                new SearchResult
                                {
                                    BusinessName = "",
                                    BusinessRegistrationNumber = "",
                                    JurisdictionOfIncorporation = ""
                                }
                            }
                        }
                        }
                    }
                }
            };
        }

        [Theory(Skip = "Calls API")]
        [MemberData(nameof(BusinessSearchResultTestData))]
        public async Task BusinessSearchResultTest(string transactionRecordId, BusinessSearchResponse expectedResponse)
        {
            using (var client = new TruliooApiClient(new Context(_username, _password) { Host = _hostEndpoint }))
            {
                var response = await client.Business.BusinessSearchResultAsync(transactionRecordId);

                Assert.Equal(expectedResponse.TransactionID, response.TransactionID);
                Assert.Equal(expectedResponse.Record.RecordStatus, response.Record.RecordStatus);
                Assert.Equal(expectedResponse.CountryCode, response.CountryCode);

                Assert.Equal(expectedResponse.Record.DatasourceResults.Count(), response.Record.DatasourceResults.Count());
                List<string> expectedDatasourcesNames = expectedResponse.Record.DatasourceResults.Select(x => x.DatasourceName).ToList();
                List<string> actualDatasourceNames = response.Record.DatasourceResults.Select(x => x.DatasourceName).ToList();
                Assert.True(expectedDatasourcesNames.All(actualDatasourceNames.Contains));

                List<string> expectedBusinessNameResults = expectedResponse.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessName)).ToList();
                List<string> actualBusinessNameResults = response.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessName)).ToList();
                Assert.Equal(expectedBusinessNameResults.Count(), actualBusinessNameResults.Count());
                Assert.True(expectedBusinessNameResults.All(actualBusinessNameResults.Contains));

                List<string> expectedBusinessNumberResults = expectedResponse.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessRegistrationNumber)).ToList();
                List<string> actualBusinessNumberResults = response.Record.DatasourceResults.SelectMany(datasource => datasource.Results.Select(result => result.BusinessRegistrationNumber)).ToList();
                Assert.Equal(expectedBusinessNumberResults.Count(), actualBusinessNumberResults.Count());
                Assert.True(expectedBusinessNumberResults.All(actualBusinessNumberResults.Contains));
            }
        }

        public static IEnumerable<object[]> BusinessSearchResultTestData()
        {
            yield return new object[] { "transactionRecordId",
                new BusinessSearchResponse()
                {
                    TransactionID = "",
                    CountryCode = "",
                    Record = new BusinessRecord()
                    {
                        RecordStatus = "",
                        DatasourceResults = new List<BusinessSearchResult>{ new BusinessSearchResult {
                            DatasourceName = "",
                            Results = new List<SearchResult>
                            {
                                new SearchResult
                                {
                                    BusinessName = "",
                                    BusinessRegistrationNumber = "",
                                    JurisdictionOfIncorporation = ""
                                }
                            }
                        }
                        }
                    }
                }
            };
        }
    }
}