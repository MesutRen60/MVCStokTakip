$(document).ready(function () {
    $('#tblKategori').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });
});



$(document).ready(function () {
    $('#tblMarkalar').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });
});

$(document).ready(function () {
    $('#tblBIRIM').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
            //"sProcessing": "İşleniyor...",
            //"sLengthMenu": "Sayfada _MENU_ Kayıt Göster",
            //"sZeroRecords": "Eşleşen Kayıt Bulunmadı",
            //"sInfo": "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
            //"sInfoEmpty": "Kayıt Yok",
            //"sInfoFiltered": "( _MAX_ Kayıt İçerisinden Bulunan)",
            //"sInfoPostFix": "",
            //"sSearch": "Bul:",
            //"sUrl": "",
            //"oPaginate": {
            //    "sFirst": "İlk",
            //    "sPrevious": "ÖNCEKİ",
            //    "sNext": "Sonraki",
            //    "sLast": "Son"
        }
    });
});

$(document).ready(function () {
    $('#tblUrun').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });
});

$(document).ready(function () {
    $('#tblSepet').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });
});


//Sweetalert için kullandım Marklar Index sayfasında 
$(function () {
    $("#tblMarkalar").on("click", ".btnMarkasil", function () {
        var btn = $(this);

        Swal.fire({
            title: 'Markayı silmek istediğinize emin misiniz??',
            text: "İşlemi onaylarsanız Bunu geri alamayacaksın!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, Markayı Sil!'
        }).then((result) => {
            if (result.isConfirmed) {
                var id = btn.data("id");
                $.ajax({
                    type: "GET",
                    url: "/Markalar/MarkalarSil/" + id,
                    success: function () {
                        btn.parent().parent().remove();
                        Swal.fire(
                            'Silindi!',
                            'Silme İşlmi başarılı',
                            'success'
                        )
                    }
                });

            };
        });
    });
});
