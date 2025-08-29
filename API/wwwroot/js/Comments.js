//Yorum yapma fonksiyonu
$(document).ready(function () {
    $(document).on('submit', '[data-form="comment-form"]', function (e) {
        e.preventDefault();
        const postId = $(this).find('textarea').attr('data-postId');
        const textarea = $(this).find('textarea');
        $.ajax({
            type: 'POST',
            url: "/Post/AddComment",
            datatype: 'json',
            headers: { "RequestVerificationToken": $(this).find('input[name="__RequestVerificationToken"]').val() },
            data: {
                PostId: postId,
                UserName: "@(User.Identity.Name)",
                Text: textarea.val()
            },
            success: function (yorum) {

                var date = new Date(yorum.publishedOn);
                $(`[data-comments="${postId}"]`).append(`
                        <div class="mb-7">
                            <div class="d-flex mb-5">
                                <div class="symbol symbol-45px me-5">
                                    <img style="Object-fit:cover;" src="/img/${yorum.avatar}" />
                                </div>
                                <div class="d-flex flex-column flex-row-fluid">
                                    <div class="d-flex align-items-center flex-wrap mb-1">
                                        <a href="" class="text-gray-800 text-hover-primary fw-bold me-2"> ${yorum.username}</a>
                                        <span class="text-gray-500 fw-semibold fs-7">${date.toLocaleString('tr-TR')}</span>
                                        <a href="#" class="ms-auto text-gray-500 text-hover-primary fw-semibold fs-7">Yanıt</a>
                                    </div>
                                    <span class="text-gray-800 fs-7 fw-normal pt-1">${yorum.text} </span>
                                </div>
                            </div>
                        </div>
                    `);
                textarea.val('');

                var adet = parseInt($(`[data-comment-count="${postId}"]`).text());
                $(`[data-comment-count="${postId}"]`).text(`${adet + 1} yorum`);
            },
            error: function (err) {
                console.log("Error:", err);
            }
        });
    });
});