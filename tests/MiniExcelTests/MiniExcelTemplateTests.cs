﻿using MiniExcelLibs;
using MiniExcelLibs.Tests.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace MiniExcelTests
{
    public class MiniExcelTemplateTests
    {

        public class TestIEnumerableTypeVO
        {
            public string @string { get; set; }
            public int? @int { get; set; }
            public decimal? @decimal { get; set; }
            public double? @double { get; set; }
            public DateTime? datetime { get; set; }
            public bool? @bool { get; set; }
            public Guid? Guid { get; set; }
        }
        [Fact]
        public void TestIEnumerableType()
        {
            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestIEnumerableType.xlsx";

                //1. By POCO
                var vo = new TestIEnumerableTypeVO { @string = "string", @int = 123, @decimal = decimal.Parse("123.45"), @double = (double)123.33, @datetime = new DateTime(2021, 4, 1), @bool = true, @Guid = Guid.NewGuid() };
                var value = new
                {
                    Ts = new[] {
                        vo,
                        new TestIEnumerableTypeVO{},
                        //null,
                        new TestIEnumerableTypeVO{},
                        vo
                    }
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var rows = MiniExcel.Query<TestIEnumerableTypeVO>(path).ToList();
                Assert.Equal(vo.@string, rows[0].@string);
                Assert.Equal(vo.@int, rows[0].@int);
                Assert.Equal(vo.@double, rows[0].@double);
                Assert.Equal(vo.@decimal, rows[0].@decimal);
                Assert.Equal(vo.@bool, rows[0].@bool);
                Assert.Equal(vo.datetime, rows[0].datetime);
                Assert.Equal(vo.Guid, rows[0].Guid);

                Assert.Null(rows[1].@string);
                Assert.Null(rows[1].@int);
                Assert.Null(rows[1].@double);
                Assert.Null(rows[1].@decimal);
                Assert.Null(rows[1].@bool);
                Assert.Null(rows[1].datetime);
                Assert.Null(rows[1].Guid);

                //Assert.Null(rows[2]);


                Assert.Equal(vo.@string, rows[3].@string);
                Assert.Equal(vo.@int, rows[3].@int);
                Assert.Equal(vo.@double, rows[3].@double);
                Assert.Equal(vo.@decimal, rows[3].@decimal);
                Assert.Equal(vo.@bool, rows[3].@bool);
                Assert.Equal(vo.datetime, rows[3].datetime);
                Assert.Equal(vo.Guid, rows[3].Guid);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:G5", demension);
            }
        }

        [Fact]
        public void TemplateCenterEmptyTest()
        {
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
            var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateCenterEmpty.xlsx";
            var value = new
            {
                Tests = Enumerable.Range(1,5).Select((s,i)=>new { test1=i,test2=i })
            };
            MiniExcel.SaveAsByTemplate(path, templatePath, value);
        }

        [Fact]
        public void TemplateBasiTest()
        {
            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateEasyFill.xlsx";
                // 1. By POCO
                var value = new
                {
                    Name = "Jack",
                    CreateDate = new DateTime(2021, 01, 01),
                    VIP = true,
                    Points = 123
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var rows = MiniExcel.Query(path).ToList();
                Assert.Equal("Jack", rows[1].A);
                Assert.Equal("2021-01-01 00:00:00", rows[1].B);
                Assert.Equal(true, rows[1].C);
                Assert.Equal(123, rows[1].D);
                Assert.Equal("Jack has 123 points", rows[1].E);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:E2", demension);
            }

            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateEasyFill.xlsx";
                // 2. By Dictionary
                var value = new Dictionary<string, object>()
                {
                    ["Name"] = "Jack",
                    ["CreateDate"] = new DateTime(2021, 01, 01),
                    ["VIP"] = true,
                    ["Points"] = 123
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var rows = MiniExcel.Query(path).ToList();
                Assert.Equal("Jack", rows[1].A);
                Assert.Equal("2021-01-01 00:00:00", rows[1].B);
                Assert.Equal(true, rows[1].C);
                Assert.Equal(123, rows[1].D);
                Assert.Equal("Jack has 123 points", rows[1].E);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:E2", demension);
            }
        }

        [Fact]
        public void TestIEnumerable()
        {
            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateBasicIEmumerableFill.xlsx";

                //1. By POCO
                var value = new
                {
                    employees = new[] {
                        new {name="Jack",department="HR"},
                        new {name="Lisa",department="HR"},
                        new {name="John",department="HR"},
                        new {name="Mike",department="IT"},
                        new {name="Neo",department="IT"},
                        new {name="Loan",department="IT"}
                    }
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:B7", demension);
            }

            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateBasicIEmumerableFill.xlsx";

                //2. By Dictionary
                var value = new Dictionary<string, object>()
                {
                    ["employees"] = new[] {
                        new {name="Jack",department="HR"},
                        new {name="Lisa",department="HR"},
                        new {name="John",department="HR"},
                        new {name="Mike",department="IT"},
                        new {name="Neo",department="IT"},
                        new {name="Loan",department="IT"}
                    }
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:B7", demension);
            }
        }

        [Fact]
        public void TemplateTest()
        {
            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateComplex.xlsx";

                // 1. By Class
                var value = new
                {
                    title = "FooCompany",
                    managers = new[] {
                        new {name="Jack",department="HR"},
                        new {name="Loan",department="IT"}
                    },
                    employees = new[] {
                        new {name="Wade",department="HR"},
                        new {name="Felix",department="HR"},
                        new {name="Eric",department="IT"},
                        new {name="Keaton",department="IT"}
                    }
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var rows = MiniExcel.Query(path).ToList();
                Assert.Equal("FooCompany", rows[0].A);
                Assert.Equal("Jack", rows[2].B);
                Assert.Equal("HR", rows[2].C);
                Assert.Equal("Loan", rows[3].B);
                Assert.Equal("IT", rows[3].C);

                Assert.Equal("Wade", rows[5].B);
                Assert.Equal("HR", rows[5].C);
                Assert.Equal("Felix", rows[6].B);
                Assert.Equal("HR", rows[6].C);

                Assert.Equal("Eric", rows[7].B);
                Assert.Equal("IT", rows[7].C);
                Assert.Equal("Keaton", rows[8].B);
                Assert.Equal("IT", rows[8].C);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:C9", demension);
            }


            {
                var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.xlsx");
                var templatePath = @"..\..\..\..\..\samples\xlsx\TestTemplateComplex.xlsx";

                // 2. By Dictionary
                var value = new Dictionary<string, object>()
                {
                    ["title"] = "FooCompany",
                    ["managers"] = new[] {
                    new {name="Jack",department="HR"},
                    new {name="Loan",department="IT"}
                },
                    ["employees"] = new[] {
                    new {name="Wade",department="HR"},
                    new {name="Felix",department="HR"},
                    new {name="Eric",department="IT"},
                    new {name="Keaton",department="IT"}
                }
                };
                MiniExcel.SaveAsByTemplate(path, templatePath, value);

                var rows = MiniExcel.Query(path).ToList();
                Assert.Equal("FooCompany", rows[0].A);
                Assert.Equal("Jack", rows[2].B);
                Assert.Equal("HR", rows[2].C);
                Assert.Equal("Loan", rows[3].B);
                Assert.Equal("IT", rows[3].C);

                Assert.Equal("Wade", rows[5].B);
                Assert.Equal("HR", rows[5].C);
                Assert.Equal("Felix", rows[6].B);
                Assert.Equal("HR", rows[6].C);

                Assert.Equal("Eric", rows[7].B);
                Assert.Equal("IT", rows[7].C);
                Assert.Equal("Keaton", rows[8].B);
                Assert.Equal("IT", rows[8].C);

                var demension = Helpers.GetFirstSheetDimensionRefValue(path);
                Assert.Equal("A1:C9", demension);
            }

        }
    }
}