var Site;
if (!Site) {
    Site = {
        RootUrl: ""
    };
}

$().ready(function () {
    Site.RootUrl = $("#uxInput_RootUrl").val();

    $("body").on("click", "button#downloadCsv", function () {
        window.location.href = Site.RootUrl + "Home/DownloadCsv";
    });
});

//Site.GetQueryStringParameter = function (parameterName) {
//    var parameterIndex = window.location.href.indexOf(parameterName + "=");
//    var parameter = '';

//    if (parameterIndex != -1) {
//        parameterIndex += (parameterName.length + 1);
//        var parameter = window.location.href.substring(parameterIndex);

//        var ampersandIndex = parameter.indexOf("&");

//        if (ampersandIndex != -1) {
//            parameter = parameter.substring(0, ampersandIndex);
//        }
//    }

//    return parameter;
//}