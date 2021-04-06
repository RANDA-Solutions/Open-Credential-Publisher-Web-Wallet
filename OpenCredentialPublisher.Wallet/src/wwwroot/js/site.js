// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

// Display an animated loading image during every navigation event

var hideLoader = function() {
    $("#loader").hide();
}

var showLoader = function() {
    $("#loader").show();
}

jQuery(function ($) {
    console.log("running submit code");
    var submitActor = null;

    // Find all the submit buttons
    var $submitActors = $("form [type=submit]");


    // Capture the button when it is clicked
    $submitActors.click(function() {
        submitActor = this;
    });

    // Show the loader unless the actor is a download button
    $("form").each(function() {
        $(this).submit(function() {
            if (null === submitActor) {
                submitActor = $submitActors[0];
            }
        });
    });

    $(window).on("beforeunload",
        function(e) {
            if (submitActor === null || typeof submitActor === "undefined") {
                submitActor = e.target.activeElement;
            }

            if (!submitActor.classList.contains('btn-download')) {
                showLoader();
            }
        });
});

// Called after verification is complete
verify_complete = (xhr) => {
    var response = JSON.parse(xhr.responseText);
    $("#verification-result-" + response.id).html(response.message);
    $("#revocationlist-result-" + response.id).html(response.revocationsMessage);
    $('[data-toggle="tooltip"]').tooltip();
}

proof_verification_complete = (xhr) => {
    var response = JSON.parse(xhr.responseText);
    $("#verifiable-credential-proof-result").html(response.message);
    $('[data-toggle="tooltip"]').tooltip();
}
