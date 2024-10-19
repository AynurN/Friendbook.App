var $messages = $('.messages-content'),
    d, h, m,
    i = 0;

$(window).on('load', function() {
    $messages.mCustomScrollbar();
    setTimeout(function() {
        fakeMessage();
    }, 100);
    $(".message-box .emojionearea-editor").addClass("message-input");
});


function updateScrollbar() {
    $messages.mCustomScrollbar("update").mCustomScrollbar('scrollTo', 'bottom', {
        scrollInertia: 10,
        timeout: 0
    });
}

function setDate() {
    var d = new Date();


    if (m != d.getMinutes()) {
        m = d.getMinutes();
        // $(".message:last-child").append('<div class="timestamp">' + d.getHours() + ':' + m + '</div>');
        $('<div class="timestamp">' + d.getHours() + ':' + m + '</div>').appendTo($('.message:last-child'));
        $('<div class="checkmark-sent-delivered">&check;</div>').appendTo($('.message:last-child'));
        $('<div class="checkmark-read">&check;</div>').appendTo($('.message:last-child'));
    }
}

function insertMessage() {
    msg = $('.tab-pane.active .emojionearea .emojionearea-editor').html();
    if ($.trim(msg) == '') {
        return false;
    }
    $('<div class="message message-personal">' + msg + '</div>').appendTo($('.mCSB_container')).addClass('new');
    setDate();
    $('.tab-pane.active .emojionearea .emojionearea-editor').html(null);
    updateScrollbar();
    setTimeout(function() {
        fakeMessage();
    }, 1000 + (Math.random() * 20) * 100);
}

$('.message-submit').click(function() {
    insertMessage();
});

$(window).on('keydown', function(e) {
    if (e.which == 13) {
        insertMessage();
        return false;
    }
})

var Fake = [
    'I’m not sure if I agree with that. I just think that the writing could have been more thoughtful while still being beautiful, if that makes sense.',
    'You’re right about that. There wasn’t much of a story.',
    'Oh, I did, too. And I appreciate hearing your point of view.',
    'Not too bad, thanks',
    'What do you do?',
    'That\'s awesome',
    'Codepen is a nice place to stay',
    'I think you\'re a nice person',
    'Why do you think that?',
    'Can you explain?',
    'Anyway I\'ve gotta go now',
    'It was a pleasure chat with you',
    'Time to make a new codepen',
    'Bye',
    ':)'
]

function fakeMessage() {
    if ($('.message-input').val() != '') {
        return false;
    }
    $('<div class="message loading new"><span></span></div>').appendTo($('.mCSB_container'));
    updateScrollbar();

    setTimeout(function() {
        $('.message.loading').remove();
        $('<div class="message new">' + Fake[i] + '</div>').appendTo($('.mCSB_container')).addClass('new');
        setDate();
        updateScrollbar();
        i++;
    }, 1000 + (Math.random() * 20) * 100);

}


// chat input plus 
$(document).on("click", ".message-box .add-extent .animated-btn", function() {
    $(this).parents(".add-extent").toggleClass("show")
});


// responsive js
$(".messanger-section .tab-box .menu-option .info-user").on("click", function() {
    $(this).parents(".tab-box").find(".user-info").addClass("show")
});

$(".messanger-section .tab-box .user-title .user-img").on("click", function() {
    $(this).parents(".tab-box").find(".user-info").addClass("show")
});

$(".messanger-section .tab-box .user-info .back-btn").on("click", function() {
    $(this).parents(".tab-box").find(".user-info").removeClass("show")
});

var window_width = jQuery(window).width();
if ((window_width) < '576') {
    $(".messanger-section .chat-content .tab-content .tab-pane").removeClass("show active")
}

$(".messanger-section .tab-box .user-title .back-btn").on("click", function() {
    $(this).parents(".tab-pane").removeClass("show active")
});