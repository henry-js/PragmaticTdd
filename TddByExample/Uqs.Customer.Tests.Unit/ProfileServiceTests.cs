namespace Uqs.Customer.Tests.Unit;

public class ProfileServiceTests
{
    private readonly ProfileService _sut;

    public ProfileServiceTests()
    {
        _sut = new ProfileService();
    }
    [Fact]
    public void ChangeUsername_NullUsername_ArgumentNullException()
    {
        // Arrange

        // Act
        Action action = () => _sut.ChangeUsername(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
           .WithParameterName("username", "ChangeUsername()")
           .WithMessage("Null*", "message should inform caller that Null parameter aren't allowed");
    }

    [Theory]
    [InlineData("AnameOf8", true)]
    [InlineData("NameOfChar12", true)]
    [InlineData("AnameOfChar13", false)]
    [InlineData("NameOf7", false)]
    [InlineData("", false)]
    public void ChangeUsername_VariousLengthUsernames_ArgumentOutOfRangeExceptionIfInvalid
    (string username, bool isValid)
    {
        // Arrange

        // Act
        var action = () => _sut.ChangeUsername(username);

        // Assert
        if (!isValid)
        {
            action.Should().Throw<ArgumentOutOfRangeException>()
                  .WithParameterName("username")
                  .WithMessage("Length*", "length should be between 8 & 12 characters long.");
            return;
        }
        action.Should().NotThrow();
    }

    [Theory]
    [InlineData("Letter_123", true)]
    [InlineData("!The_Start", false)]
    [InlineData("InThe@Middle", false)]
    [InlineData("WithDollar$", false)]
    [InlineData("Space 123", false)]
    public void ChangeUsername_InvalidCharValidation_ArgumentOutOfRangeException
    (string username, bool isValid)
    {
        // Arrange

        // Act
        var action = () => _sut.ChangeUsername(username);

        // Assert
        if (isValid is not true)
        {
            action.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("username")
            .WithMessage("InvalidChar*");
            return;
        }
        action.Should().NotThrow();
    }
}
