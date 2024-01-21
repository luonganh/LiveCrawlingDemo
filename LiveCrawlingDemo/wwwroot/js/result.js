var lotteryController = function () {
    this.initialize = function () {
        loadLottery();
    }
   
    function loadLottery() {        
        $.ajax({
            type: "GET",
            url: "/Home/GetLatestLotteryResult",           
            dataType: "json",
            beforeSend: function () {                
            },
            success: function (response) {                
                var template = $('#result-template').html();
                var render = "";               
                render = Mustache.render(template, {                   
                    Date: response.Date,                           
                    SpecialPrize: response.SpecialPrize,
                    Prize1st: response.Prize1st                   
                });               
                if (render != undefined) {
                    $('#lotteryResult').html(render);

                }               
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

}