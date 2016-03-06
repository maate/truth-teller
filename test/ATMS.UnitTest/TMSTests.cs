using System.Collections.Generic;
using System.Linq;

using M8.ATMS.Nodes;

using NUnit.Framework;

namespace M8.ATMS.UnitTest {
  [TestFixture]
  public class TMSTests : TestBase {
    [SetUp]
    public void SetUp() {
      SetupABr();
      Setupg();
      SetupD();
      SetupE();
      Setupl();
      Setupk();
      Setupi();
      Setuph();
    }

    [Test]
    public void TestEnvironment() {
      Environment environment = Informant.GetEnvironment( "h" );
      Assert.That( environment.ContainsId( "A" ) );
      Assert.That( environment.ContainsId( "B" ) );
      Assert.That( environment.ContainsId( "D" ) );
      Assert.That( environment.Count(), Is.EqualTo( 3 ) );
    }

    [Test]
    public void TestAssumptions() {
      List<string> assumptions = Informant.GetAssumptions();
      Assert.That( assumptions.Contains( "A" ) );
      Assert.That( assumptions.Contains( "B" ) );
      Assert.That( assumptions.Contains( "D" ) );
      Assert.That( assumptions.Contains( "E" ) );
      Assert.That( assumptions.Count, Is.EqualTo( 4 ) );
    }

    [Test]
    public void TestEnvironmentExactSupport() {
      var environment = new Environment( "A", "B", "D" );
      Assert.That( Informant.IsSupported( "h", environment ) );
    }

    [Test]
    public void TestSupersetEnvironmentSupport() {
      var environment = new Environment( "A", "B", "D", "E" );
      Assert.That( Informant.IsSupported( "h", environment ) );
    }

    [Test]
    public void TestMissingEnvironmentSupport() {
      var environment = new Environment( "A", "B" );
      Assert.That( Informant.IsSupported( "h", environment ), Is.False );
    }

    [Test]
    public void TestMinimalEnvironmentSupport() {
      var environment = new Environment( "A", "D" );
      Assert.That( Informant.IsSupported( "h", environment ) );
    }

    [Test]
    public void WhenCreatingNode_ThenItsLabelIsEmpty() {
      var node = new Node();
      Assert.That( node.Label, Is.EqualTo( Label.NoEnvironment ) );
    }

    [Test]
    public void WhenNodeHasEmptyLabel_ThenItIsOut() {
      var node = new Node( "Foo" );
      Recorder.Justify( node );
      Assert.That( Informant.IsOut( node.Id ) );
    }

    [Test]
    public void WhenAddingAssumptionNode_ItHasItselfAsLabel() {
      Recorder.Assume( new Node( "Foo" ) );
      var node = Informant.GetNode( "Foo" );
      AssertLabel( "Foo", "{{Foo}}" );
    }
  }
}