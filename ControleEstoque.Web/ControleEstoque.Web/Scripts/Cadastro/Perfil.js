﻿﻿function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cbx_ativo').prop('checked', dados.Ativo);

    let lista_usuario = $('#lista_usuario');
    lista_usuario.find('input[type=checkbox]').prop('checked', false);

    if (dados.Usuarios) {
        for (let i = 0; i < dados.Usuarios.length; i++) {
            let usuario = dados.Usuarios[i],
                cbx = lista_usuario.find('input[data-id-usuario=' + usuario.Id + ']');
            if (cbx) {
                cbx.prop('checked', true);
            }
        }
    }
}

function set_focus_form() {
    $('#txt_nome').focus();
}

//function set_dados_grid(dados) {
//    return '<td>' + dados.Nome + '</td>' +
//        '<td>' + (dados.Ativo ? 'SIM' : 'NÃO') + '</td>'
//}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Ativo: $('#cbx_ativo').prop('checked'),
        idUsuarios: get_lista_usuarios_marcados()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Ativo ? 'SIM' : 'NÃO');
}

function get_lista_usuarios_marcados() {
    var ids = [],
        lista_usuario = $('#lista_usuario');

    lista_usuario.find('input[type=checkbox]').each((index, input) => {
        let cbx = $(input),
            marcado = cbx.is(':checked');

        if (marcado) {
            ids.push(parseInt(cbx.attr('data-id-usuario')));
        }
    });

    return ids;
}