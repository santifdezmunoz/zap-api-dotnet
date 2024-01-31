namespace OWASPZAPDotNetAPI.Tests;

public class ClientApiTests : IDisposable
{
    private ClientApi zap;
    public ClientApiTests()
    {
        zap = new ClientApi("127.0.0.1", 8090, "on6qbod07ssf92587pme6rd5u8");
    }
    public void Dispose()
    {
        zap.Dispose();
    }

    [Fact]
    public void When_CallApi_Is_Called_IApiResponse_IsReturned()
    {
        var response = zap.CallApi("authentication", "view", "getSupportedAuthenticationMethods", null);
        Assert.IsAssignableFrom<IApiResponse>(response);
    }

    [Fact]
    public void When_CallApi_getSupportedAuthenticationMethods_Is_Called_ApiResponseList_IsReturned()
    {
        var response = zap.CallApi("authentication", "view", "getSupportedAuthenticationMethods", null);
        Assert.IsAssignableFrom<ApiResponseList>(response);
    }

    [Fact]
    public void When_CallApi_getSupportedAuthenticationMethods_Is_Called_ApiResponseList_With_formBasedAuthentication_IsReturned()
    {
        var response = zap.CallApi("authentication", "view", "getSupportedAuthenticationMethods", null);
        bool formBasedAuthenticationFound = false;
        ApiResponseList apiResponseList = (ApiResponseList)response;
        foreach (var item in apiResponseList.List)
        {
            var apiResponseElement = (ApiResponseElement)item;
            if (apiResponseElement.Value == "formBasedAuthentication")
            {
                formBasedAuthenticationFound = true;
                break;
            }
        }
        Assert.True(formBasedAuthenticationFound);
    }

    [Fact]
    public void When_CallApi_alerts_Is_Called_ApiResponseList_Is_Returned()
    {
        var response = zap.CallApi("core", "view", "alerts", null);
        ApiResponseList apiResponseList = (ApiResponseList)response;
        Assert.IsAssignableFrom<ApiResponseList>(response);
    }

    [Fact]
    public void When_CallApi_scanners_Is_Called_ApiResponseList_WithApiResponseSet_IsReturned()
    {
        var response = zap.CallApi("pscan", "view", "scanners", null);
        Assert.IsAssignableFrom<ApiResponseList>(response);
        Assert.IsAssignableFrom<ApiResponseSet>(((ApiResponseList)response).List[0]);
    }

    [Fact]
    public void When_CallApi_authentication_With_NonExistantMethod_Is_Called_Exception_Thrown()
    {
        //arrange

        //act
        Action act = () => zap.CallApi("authentication", "view", "aaaa", null);

        //assert
        Exception ex = Assert.Throws<Exception>(act);
        Assert.StartsWith("bad_view", ex.Message);

    }

    [Fact]
    public void When_Api_getForcedUser_With_NonExistantContext_Is_Called_Exception_Thrown()
    {
        //arrange

        //act
        Action act = () => zap.forcedUser.getForcedUser("-1");

        //assert
        Exception ex = Assert.Throws<Exception>(act);
        Assert.StartsWith("context_not_found", ex.Message);
    }

    [Fact]
    public void When_Api_setMode_With_NonAllowedValue_Is_Called_Exception_Thrown()
    {
        //arrange

        //act
        Action act = () => zap.core.setMode("ModeThatDoentExist");

        //assert
        Exception ex = Assert.Throws<Exception>(act);
        Assert.StartsWith("illegal_parameter", ex.Message);
    }

    [Fact]
    public void When_Api_setMode_With_Standard_Is_Called_ApiResponse_OK_Is_Returned()
    {
        IApiResponse response = zap.core.setMode("Standard");
        Assert.Equal("OK", ((ApiResponseElement)response).Value);
    }

    [Fact]
    public void When_Api_stopAllScans_Is_Called_ApiResponse_OK_Is_Returned()
    {
        IApiResponse response = zap.spider.stopAllScans();
        Assert.Equal("OK", ((ApiResponseElement)response).Value);
    }
}