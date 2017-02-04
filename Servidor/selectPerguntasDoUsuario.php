<?php
    include("conexao.php");

    $pergunta = $_GET['id_usuario'];

    $select = "SELECT h.id as 'h_id', h.acertou, h.data, u.nome, u.email, u.data_nascimento, p.tipo
                FROM historico h
                LEFT JOIN usuario u
                ON u.id = h.id_usuario
                LEFT JOIN perguntas p
                ON p.id = h.id_pergunta
                WHERE h.id_usuario =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $acertou = $pergunta['acertou'];
            $data = $pergunta['data'];
            $nome = $pergunta['nome'];
            $data_nascimento = $pergunta['data_nascimento'];
            $email = $pergunta['email'];
            $tipo = $pergunta['tipo'];

            echo '&id='.$id.'#'.$acertou.'#'.$data.'#'.$nome.'#'.$data_nascimento.'#'.$email.'#'.$tipo;
        }
    }
    else{
        echo 0;
    }
?>