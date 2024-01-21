var connection = new signalR.HubConnectionBuilder()
    .withUrl("/UpdateLotteryHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));
connection.serverTimeoutInMilliseconds = 100000; // 100 second

//connection.client.refreshPage = function () {
//    $.ajax({
//        url: "/live/update",
//        cache: false,
//        dataType: "html",
//        success: function (data) {
//            $("#containerToUpdate").html(data);
//        }
//    });
//};

function normalizeInput(stringData) {
    if (stringData) {
        return stringData.toLowerCase().replace(/&nbsp;/g, "");
    }
    return "";
}
function formatStringToRequireDate(input) {
        var datePart = input.match(/\d+/g),
            year = datePart[0].substring(0),
            month = datePart[1],
            day = datePart[2];
        return day + '-' + month + '-' + year;   
}
function checkDataResponse(input) {
    if (input == "*" || input == "&nbsp;" || !input)
        return true;
    else
        return false;
}

connection.on("TraditionalLottery", (message) => {    
    var template = $('#result-template').html();
    Mustache.parse(template);
    var html = Mustache.render(template, {              
        Date: message.date,    
        Prize0: message.prize0,
        Prize1: message.prize1        
    });
    $('#lotteryResult').html(html);
    console.log(message);
});

