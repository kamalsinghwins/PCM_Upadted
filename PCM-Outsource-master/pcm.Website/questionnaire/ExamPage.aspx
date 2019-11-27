<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExamPage.aspx.vb" Inherits="pcm.Website.ExamPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rage Questionnaire</title>
    <script type="text/javascript" src="../js/General/jquery-2.0.3.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css" />
    <style type="text/css">
        .text-center {
            text-align: center;
        }
        .mb-10 {
            margin-bottom: 10px;
        }
        .mt-10 {
            margin-top: 10px;
        }
        .mt-20 {
            margin-top: 20px;
        }
        #mainContainer {
            padding: 15px 20px;
        }
        .question_block {
            margin: 20px 0;
            font-size: 14px;
            background: #eaf2fb;
            padding: 20px;
            box-sizing: border-box;
        }
            .question_block .large {
                font-weight: bold;
                margin-bottom: 20px;
            }
        .survey-container {
            display: flex;
            flex-direction: row;
            align-items: center;
            overflow-y: auto;
            bottom: 60px;
            top: 60px;
            background: #fff;
            position: absolute;
            width: 100%;
        }
            .survey-container .question-box {
                width: 680px;
                max-width: 100%;
                padding: 0 15px;
                margin: 0 auto;
                padding: 25px;
                max-height: 100%;
            }
        .footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            height: 60px;
            line-height: 60px;
            background-color: #f5f5f5;
        }
        .pt30 {
            padding-top: 30px;
        }
        .pb60 {
            padding-bottom: 60px;
        }
        .survey-question {
            font-size: 18px;
            font-weight: bold;
            width:100%;
            overflow-wrap :break-word 
        }
        ul {
            list-style: none;
            margin: 0;
            padding: 0;
        }
        span {
            font-weight: 400;
            font-size: 16px;
        }
        .survey-answer {
            margin-top: 20px;
            margin-bottom: 20px;
            border-radius: 4px;
            font-size: 15px;
            padding-left: 0;
            position: relative;
            display: inline-block;
    width: 100%;
    overflow-wrap :break-word 
        }
            .survey-answer label {
                min-height: 21px;
                padding-left: 20px;
                margin-bottom: 0;
                font-weight: 400;
                cursor: pointer;
                padding: 15px;
                border: 1px solid #cacbcc;
                border-radius: 4px;
                width: 100%;
                display: inline-block;
                max-width: 100%;
                overflow-wrap :break-word 
            }
                .survey-answer label:hover {
                    background: #f2f3f5;
                }
            .survey-answer .radio-label {
                display: inline-block;
                line-height: 1.43;
                position: relative;
                padding-left: 1.67em;
                padding-top: .07em;
                cursor: pointer;
                -webkit-touch-callout: none;
                -webkit-user-select: none;
                -khtml-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                overflow-wrap :break-word;
                        max-width: 90%;
    vertical-align: middle;
            }
        #message {
            transition: 0.3s ease all;
        }
        .jumbotron{
    border-radius:0px!important;
    box-shadow:1px 1px 4px rgba(0,0,0,0.4);
    background-size: cover;
    background-position: center center;
    background-repeat: no-repeat;
    background-image: url(https://static.pexels.com/photos/24324/pexels-photo.jpg); 
    position: relative;
    height: 100%;


}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Fixed navbar -->
        <nav class="navbar navbar-default navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="javascript:void(0)"> <%= employeeSession.SurveyName  %> Questionnaire</a>
                </div>
                <div id="navbar" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="javascript:void(0)"><%= employeeSession.FirstName & " " & employeeSession.LastName  %></a></li>
                    </ul>
                </div>
            </div>
        </nav>

        <div class="survey-container">
            <div class="question-box">
                <div id="message" class="text-center"></div>
                <div class="pt30 pb60">
                    <div class="survey-question-header">
                        <span class="pull-left current-question"></span>
                        <span class="pull-right"><strong id="timer"></strong> seconds</span>
                        <p class="survey-question mb-10 pt30 pull-left"></p>
                    </div>
                    <ul class="options_block">
                    </ul>
                </div>
            </div>
        </div>
        <!-- /container -->
        <footer class="footer">
            <div class="container">
                <div class="pull-left">
                <span class="question-breadcrumb"></span>
                </div>
                <div class="pull-right action_buttons_block">

                </div>
            </div>
        </footer>

    </form>
    <script>
        $(document).ready(function() {
            var RemainingTimeInSeconds = <%= RemainingTime %>;
            var min = Math.floor(RemainingTimeInSeconds / 60);
            var sec = RemainingTimeInSeconds - min * 60;
            document.getElementById('timer').innerHTML = min + ":" + sec;
            if(RemainingTimeInSeconds > 0) {
                startTimer();
                $.ajax({
                    url: 'ExamPage.aspx/GetSingleQuestion',
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: renderHtml
                });
            }
        });
        function startTimer() {
            if($('#timer').length > 0) {
                var presentTime = document.getElementById('timer').innerHTML;
                var timeArray = presentTime.split(/[:]+/);
                var m = timeArray[0];
                var s = checkSecond((timeArray[1] - 1));
                if(s==59){m=m-1}
                if (m < 0) {
                       $(".question-box .pt30.pb60").remove();
                            $(".action_buttons_block").remove();
                            $(".question-breadcrumb").remove();
                    $.ajax({

                        url: 'ExamPage.aspx/SurveyTimeout',
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {   
                            $(".question-box .pt30.pb60").remove();
                            $(".action_buttons_block").remove();
                            $(".question-breadcrumb").remove();
                                var html = "";                           
                   html += '<div >\
	                                    <div class="row">\
		                            <div class="jumbotron">\
                                      <h1>Result</h1>\
                                      <p>Total Question : '+ data.d.TotalQuestions +'</p>\
                                      <p>Score : '+ data.d.Score +'</p>\
                                     </div></div></div>';

                        $("#message").html(html);
                        }
                    });
                } else {
                    document.getElementById('timer').innerHTML = m + ":" + s;
                    setTimeout(startTimer, 1000);
                }
            }
        }
        function checkSecond(sec) {
            if (sec < 10 && sec >= 0) {sec = "0" + sec}; // add zero in front of numbers < 10
            if (sec < 0) {sec = "59"};
            return sec;
        }
        function next() {
            var IsAnswerRadioChecked = $(".options_block input[name='answer']").is(":checked");
            if (!IsAnswerRadioChecked) {
                $("#message").html("Please select answer of question.").addClass("alert alert-danger");
                setTimeout(function() { 
                    $("#message").html("").removeClass("alert alert-danger");
                }, 2000);
                return false;
            }
         
            $('#btn_next, #btn_previous, #btn_finish').attr('disabled', 'disabled');
            var option_id = $(".options_block input[name='answer']:checked").val();
            $.ajax({
                url: 'ExamPage.aspx/Increment',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: '{option_id:"' + option_id + '"}',
                dataType: 'json',
                success: renderHtml
            });
        }
        function previous() {
            $('#btn_next, #btn_previous, #btn_finish').attr('disabled', 'disabled');
            $.ajax({
                url: 'ExamPage.aspx/Decrement',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: renderHtml
            });
        }
        function finish() {
            var IsAnswerRadioChecked = $(".options_block input[name='answer']").is(":checked");
            if (!IsAnswerRadioChecked) {
                $("#message").html("Please select answer of question.").addClass("alert alert-danger");
                setTimeout(function() { 
                    $("#message").html("").removeClass("alert alert-danger");
                }, 2000);
                return false;
            }
                            var html = "";
            
            $('#btn_next, #btn_previous, #btn_finish').attr('disabled', 'disabled');
            var option_id = $(".options_block input[name='answer']:checked").val();
            $.ajax({
                url: 'ExamPage.aspx/Finish',
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: '{option_id:"' + option_id + '"}',
                dataType: 'json',
                success: function(data) {
                    if (data.d.Success == true) {
                          var html = "";                           
                   html += '<div >\
	                                    <div class="row">\
		                            <div class="jumbotron">\
                                      <h1>Result</h1>\
                                      <p>Total Question : '+ data.d.TotalQuestions +'</p>\
                                      <p>Score : '+ data.d.Score +'</p>\
                                     </div></div></div>';
              
                        $("#message").html(html);



                        $(".question-box .pt30.pb60").remove();
                        $(".action_buttons_block").remove();
                        $(".question-breadcrumb").remove();
                    } else {
                        $("#message").html(data.d.Message).addClass("alert alert-danger");
                    }
                }
            });
        }
        function renderHtml(data) {     
            $('#btn_next, #btn_previous, #btn_finish').removeAttr('disabled');
            if(data.d.Success == true) {
                var html = "";
                var QuestionDetail = data.d.SurveyQuestions;
                var CurrentQuestion = data.d.CurrentQuestion;
                var TotalQuestions = data.d.TotalQuestions;
                var IsFirst = data.d.IsFirst;
                var IsLast = data.d.IsLast;
                var QuestionOptionList = [];
                $(".survey-question").text(QuestionDetail.question_text);
                QuestionOptionList = QuestionDetail.Options;
                for (j = 0; j < QuestionOptionList.length; j++) {
                    html += '<li>\
                                        <div class="survey-answer">\
                                            <label>\
                                                <input type="radio" name="answer" value="' + QuestionOptionList[j].option_id + '" ' + (QuestionOptionList[j].option_id == QuestionDetail.selected_option ? 'checked' : '') + '  > <span class="radio-label">' + QuestionOptionList[j].option_text + ' </span>\
                                            </label>\
                                        </div>\
                                    </li>';
                }
                $(".options_block").html(html);
                html = "";
                if (IsFirst == true) {
                    html += '<a class="btn btn-danger" id="btn_next"  onclick="next(this)">Next</a>';
                } else if (IsLast == true) {
                    html += '<a class="btn btn-primary" id="btn_previous"  onclick="previous(this)">Previous</a>\
                                                  <a class="btn btn-success" id="btn_finish" onclick="finish(this)">Finish</a>';
                } else {
                    html += '<a class="btn btn-primary" id="btn_previous"  onclick="previous(this)">Previous</a>\
                                                  <a class="btn btn-danger" id="btn_next" onclick="next(this)">Next</a>';
                }
                $(".action_buttons_block").html(html);
                $(".question-breadcrumb").text("Question "+ CurrentQuestion+" of "+ TotalQuestions);
                $(".current-question").text("Question "+ CurrentQuestion+ ":");
            }
            else 
            {
                $("#message").html(data.d.Message).addClass("alert alert-danger");
            }
        }
    </script>
</body>
</html>