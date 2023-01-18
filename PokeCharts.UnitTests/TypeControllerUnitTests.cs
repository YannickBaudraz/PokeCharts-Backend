using PokeCharts.Models;
using PokeCharts.Daos;
//using PokeCharts.Controllers;
using Moq;
using PokeCharts.Controllers;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests;

public class TypeControllerUnitTests
{
    TypesController typesController;
    Mock<ITypeDao> typeDaoMock;
    
    List<Type> doubleDamageTo = new()
        {
            new Type(1, "normal"),
            new Type(2,"fighting"),
            new Type(3,"flying")
        };
    List<Type> halfDamageTo = new()
        { 
            new Type(4, "fire"),
            new Type(5, "water"),
            new Type(6, "grass")
        };
    List<Type> noDamageTo = new()
        {
            new Type(7,"electric"),
            new Type(8,"psychic"),
            new Type(9,"ice")
        };
    
    [SetUp]
    public void OneTimeSetUp()
    {
        typeDaoMock = new Mock<ITypeDao>();
        typesController = new TypesController(typeDaoMock.Object);
    }

    [Test]
    public void GetAllNominalCaseSuccess()
    {
        //given
        List<Type> types = new()
        {
            new Type(1, "normal"),
            new Type(4, "fire"),
            new Type(7, "water"),
            new Type(25, "electric")
        };
        typeDaoMock.Setup(m => m.Get()).Returns(types);
        
        //when
        List<Type>? results = typesController.GetAll().Value;

        //then
        Assert.That(results, Is.EqualTo(types));
    }
    [Test]
    public void GetNameSuccess()
    {
        //given
        Type expectedType = new Type(1, "normal", doubleDamageTo, halfDamageTo, noDamageTo);
        typeDaoMock.Setup(m => m.Get("normal")).Returns(expectedType);
        
        //when
        Type? results = typesController.Get("normal").Value;

        //then
        Assert.That(results, Is.EqualTo(expectedType));
    }
    [Test]
    public void GetNameException()
    {
        //given
        typeDaoMock.Setup(m => m.Get("yellow")).Throws(new Exception("type does not exist"));

        //when & then
        Assert.Throws<Exception>(() => typesController.Get("yellow"));
    }
    [Test]
    public void GetIdSuccess()
    {
        Type expectedType = new Type(1, "normal", doubleDamageTo, halfDamageTo, noDamageTo);
        typeDaoMock.Setup(m => m.Get(1)).Returns(expectedType);

        //when
        Type? results = typesController.Get(1).Value;

        //then
        Assert.That(results, Is.EqualTo(expectedType));
    }
    [Test]
    public void GetIdException()
    {
        //given
        typeDaoMock.Setup(m => m.Get(-1)).Throws(new Exception("type does not exist"));

        //when & then
        Assert.Throws<Exception>(() => typesController.Get(-1));
    }
}