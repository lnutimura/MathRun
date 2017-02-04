<?php
    include("conexao.php");

    $pergunta = $_GET['id_usuario'];

    $select = "SELECT h.id, h.id_fase, p.tipo, h.acertou, h.data, u.id as user_id, u.nome, u.data_nascimento, u.email
								FROM historico h, usuario u, perguntas p
								WHERE h.id_pergunta = p.id AND u.id =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nome = $pergunta['nome'];
            $data_nascimento = $pergunta['data_nascimento'];
            $email = $pergunta['email'];
            $id_fase = $pergunta['id_fase'];
            $id_pergunta = $pergunta['id_pergunta'];
            $acertou = $pergunta['acertou'];
            $data = $pergunta['data'];

            echo '&id='.$id.'#'.$nome.'#'.$data_nascimento.'#'.$email.'#'.$id_fase.'#'.$id_pergunta.'#'.$acertou.'#'.$data;
        }
    }
    else{
        echo 0;
    }
?>