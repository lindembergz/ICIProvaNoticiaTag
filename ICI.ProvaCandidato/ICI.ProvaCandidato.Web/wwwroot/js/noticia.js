$(document).ready(function () {

    $('#btnNovaNoticia').click(function () {
        $.get('/Noticia/Create', function (data) {
            $('#modalFormulario .modal-body').html(data);
            $('#modalFormulario').modal('show');
        }).fail(function () {
            alert('Erro ao carregar formulário');
        });
    });


    $("#formNoticia").validate({
        rules: {
            Titulo: {
                required: true,
                maxlength: 200
            },
            Conteudo: {
                required: true
            }
        },
        messages: {
            Titulo: {
                required: "Título é obrigatório",
                maxlength: "Título não pode exceder 200 caracteres"
            },
            Conteudo: {
                required: "Conteúdo é obrigatório"
            }
        },
        errorPlacement: function (error, element) {
            error.addClass("text-danger");
            error.insertAfter(element);
        },
        submitHandler: function (form) {
            const formData = {
                Id: $('#Id').val(),
                Titulo: $('#Titulo').val(),
                Conteudo: $('#Conteudo').val(),
                TagIdsSelecionadas: []
            };

            $('input[name="TagIdsSelecionadas"]:checked').each(function () {
                formData.TagIdsSelecionadas.push(parseInt($(this).val()));
            });

            const url = formData.Id > 0 ? '/Noticia/Edit' : '/Noticia/Create';

            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(formData),
                success: function (response) {
                    if (response.sucesso) {
                        $('#modalFormulario').modal('hide');
                        location.reload();
                    } else {
                        exibirErros(response.erros);
                    }
                },
                error: function () {
                    alert('Erro ao processar requisição');
                }
            });
        }
    });


    $('.btn-editar').click(function () {
        const id = $(this).data('id');
        $.get('/Noticia/Edit/' + id, function (data) {
            $('#modalFormulario .modal-body').html(data);
            $('#modalFormulario').modal('show');
        }).fail(function () {
            alert('Erro ao carregar formulário');
        });
    });


    $(document).on('submit', '#formNoticia', function (e) {

        console.log("Form submitted");

        e.preventDefault();

        const formData = {
            Id: $('#Id').val(),
            Titulo: $('#Titulo').val(),
            Conteudo: $('#Conteudo').val(),
            TagIdsSelecionadas: []
        };


        $('input[name="TagIdsSelecionadas"]:checked').each(function () {
            formData.TagIdsSelecionadas.push(parseInt($(this).val()));
        });

        const url = formData.Id > 0 ? '/Noticia/Edit' : '/Noticia/Create';

        if (!formData.Titulo || !formData.Conteudo) {
            exibirErros({ Geral: ["Título e conteúdo são obrigatórios"] });
            return;
        }

        console.log(url)

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function (response) {
                if (response.sucesso) {
                    $('#modalFormulario').modal('hide');
                    location.reload(); 
                } else {
                    exibirErros(response.erros);
                }
            },
            error: function () {
                alert('Erro ao processar requisição');
            }
        });
    });

    function exibirErros(erros) {
        $('.text-danger').remove();

        for (const campo in erros) {
            const mensagens = erros[campo];
            const elemento = $(`#${campo}`);

            if (elemento.length) {
                mensagens.forEach(function (mensagem) {
                    elemento.after(`<span class="text-danger">${mensagem}</span>`);
                });
            }
        }
    }


    $('.btn-excluir').click(function () {
        const id = $(this).data('id');
        $.get('/Noticia/Delete/' + id, function (data) {
            $('#modalFormulario .modal-body').html(data);
            $('#modalFormulario').modal('show');
        }).fail(function () {
            alert('Erro ao carregar formulário de exclusão');
        });
    });



    $(document).on('click', '.btn-excluir-confirmar', function () {
        const id = $(this).data('id');
        console.log('Deleting ID:', id);
        if (!id || id <= 0) {
            alert('ID inválido para exclusão');
            return;
        }
        $.ajax({
            url: '/Noticia/Delete',
            type: 'POST', 
            contentType: 'application/json',
            data: JSON.stringify({ id: parseInt(id) }), 
            success: function (response) {
                if (response.sucesso) {
                    $('#modalFormulario').modal('hide');
                    location.reload();
                } else {
                    alert(response.mensagem || 'Erro ao excluir notícia');
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro AJAX:', status, error);
                alert('Erro ao processar requisição');
            }
        });
    });


    // Search Form Submission
    $('#formPesquisa').on('submit', function (e) {
        e.preventDefault();
        const termo = $('#termoPesquisa').val().trim();
        const url = '/Noticia/Index' + (termo ? `?termoPesquisa=${encodeURIComponent(termo)}` : '');
        window.location.href = url;
    });


});