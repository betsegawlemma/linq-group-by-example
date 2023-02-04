using dotnet_experiments;

var woredas = new List<string>
{
    "Woreda 1",
    "Woreda 2",
    "Woreda 3"
};

var populations = new List<Population>
{
    new()
    {
        Woreda = "Woreda 1",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 1998
    },
    new()
    {
        Woreda = "Woreda 1",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 1830
    },
    new()
    {
        Woreda = "Woreda 1",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 49771
    },
    new()
    {
        Woreda = "Woreda 1",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 51125
    },

    new()
    {
        Woreda = "Woreda 2",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 2096
    },
    new()
    {
        Woreda = "Woreda 2",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 1981
    },
    new()
    {
        Woreda = "Woreda 2",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 14867
    },
    new()
    {
        Woreda = "Woreda 2",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 15359
    },

    new()
    {
        Woreda = "Woreda 3",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 5040
    },
    new()
    {
        Woreda = "Woreda 3",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Urban,
        NumberOfPopulation = 4940
    },
    new()
    {
        Woreda = "Woreda 3",
        Gender = Gender.Male,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 29632
    },
    new()
    {
        Woreda = "Woreda 3",
        Gender = Gender.Female,
        ResidencyType = ResidencyType.Rural,
        NumberOfPopulation = 30666
    }
};

var totalByGender = from p in populations
    group p by new { p.Gender, p.Woreda }
    into g
    select new GenderAggregate
        { Woreda = g.Key.Woreda, Gender = g.Key.Gender.ToString(), Total = g.Sum(p => p.NumberOfPopulation) };

var totalByResidencyType = from p in populations
    group p by new { p.ResidencyType, p.Woreda }
    into g
    select new ResidentTypeAggregate
    {
        Woreda = g.Key.Woreda, ResidentType = g.Key.ResidencyType.ToString(), Total = g.Sum(p => p.NumberOfPopulation)
    };

var totalByWoreda = from p in populations
    group p by new { p.Woreda }
    into g
    select new { Woreda = g.Key.Woreda, Total = g.Sum(p => p.NumberOfPopulation) };

const string rowSeparator =
    "----------------------------------------------------------------------------------------------------------------------------------";
var header1 =
    $"{"",10} | {"",10}  {"Urban",10}  {"",12} | {"",10}  {"Rural",10}  {"",12} | {"",10}  {"Urban + Rural",10}  {"",10}";
var header2 =
    $"{"Woreda",10} | {"Male",10} | {"Female",10} | {"Total",10} | {"Male",10} | {"Female",10} | {"Total",10} | {"Male Total",10} | {"Female Total",10} | {"Total",10}";

Console.WriteLine(rowSeparator);
Console.WriteLine(header1);
Console.WriteLine(rowSeparator);
Console.WriteLine(header2);
Console.WriteLine(rowSeparator);

foreach (var woreda in woredas)
{
    var urbanMaleValue = populations.First(p =>
        p.Woreda == woreda && p is { Gender: Gender.Male, ResidencyType: ResidencyType.Urban });
    var urbanFemaleValue = populations.First(p =>
        p.Woreda == woreda && p is { Gender: Gender.Female, ResidencyType: ResidencyType.Urban });
    var urbanTotal = totalByResidencyType
        .FirstOrDefault(a => a.ResidentType == ResidencyType.Urban.ToString() && a.Woreda == woreda).Total;
    var ruralMaleValue = populations.First(p =>
        p.Woreda == woreda && p is { Gender: Gender.Male, ResidencyType: ResidencyType.Rural });
    var ruralFemaleValue = populations.First(p =>
        p.Woreda == woreda && p is { Gender: Gender.Female, ResidencyType: ResidencyType.Rural });
    var ruralTotal = totalByResidencyType
        .FirstOrDefault(a => a.ResidentType == ResidencyType.Rural.ToString() && a.Woreda == woreda).Total;

    var maleTotal = totalByGender.FirstOrDefault(p => p.Woreda == woreda && p.Gender == Gender.Male.ToString()).Total;
    var femaleTotal = totalByGender.FirstOrDefault(p => p.Woreda == woreda && p.Gender == Gender.Female.ToString())
        .Total;

    var total = totalByWoreda.FirstOrDefault(p => p.Woreda == woreda).Total;

    Console.WriteLine(
        $"{woreda,10} | {urbanMaleValue.NumberOfPopulation,10} | {urbanFemaleValue.NumberOfPopulation,10} | {urbanTotal,10} | {ruralMaleValue.NumberOfPopulation,10} | {ruralFemaleValue.NumberOfPopulation,10} | {ruralTotal,10} | {maleTotal,10} | {femaleTotal,12} | {total, 10}");
    Console.WriteLine(rowSeparator);
}