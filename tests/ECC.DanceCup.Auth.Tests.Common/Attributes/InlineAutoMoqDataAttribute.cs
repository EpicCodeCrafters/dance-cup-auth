using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ECC.DanceCup.Auth.Domain.Tests.Common.Attributes;

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object?[] objects)
        : base(() => new Fixture().Customize(new AutoMoqCustomization()), objects)
    {
    }
}