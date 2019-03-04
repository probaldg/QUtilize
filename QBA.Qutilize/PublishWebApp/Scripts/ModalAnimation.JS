function testAnim(x) {
    $('.modal .modal-dialog').attr('class', 'modal-dialog  ' + x + '  animated');
};


function testAnimBig(x) {   
    $('.modal .modal-dialog-big').attr('class', 'modal-dialog-big  ' + x + '  animated');
};

$('#myModal2').on('show.bs.modal', function (e) {
    var anim = "slideInRight";
    testAnim(anim);
})
$('#myModal2').on('hide.bs.modal', function (e) {
    var anim = "slideOutRight";
    testAnim(anim);
})


$('#publishedModal').on('show.bs.modal', function (e) {
    var anim = "slideInRight";
    testAnimBig(anim);
})
$('#publishedModal').on('hide.bs.modal', function (e) {
    var anim = "slideOutRight";
    testAnimBig(anim);
})

$('#myModalUnpublishedPreview').on('show.bs.modal', function (e) {
    var animBig = "slideInRight";
    testAnimBig(animBig);
})
$('#myModalUnpublishedPreview').on('hide.bs.modal', function (e) {
    var animBig = "slideOutRight";
    testAnimBig(animBig);
})



$('#rejectedWikiModal').on('show.bs.modal', function (e) {
    var animBig = "slideInRight";
    testAnimBig(animBig);
})
$('#rejectedWikiModal').on('hide.bs.modal', function (e) {
    var animBig = "slideOutRight";
    testAnimBig(animBig);
})