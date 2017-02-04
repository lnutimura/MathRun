<?php
    include("conexao.php");

    $pergunta = $_GET['id_fase'];

    $select = "SELECT h.id, u.nome, h.acertou, h.data
                FROM historico h, usuario u
                WHERE h.id_usuario = u.id AND h.id_fase =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nomeAluno = $pergunta['nomeAluno'];
            $acertou = $pergunta['acertou'];
            $data_nascimento = $pergunta['data_nascimento'];

            echo '&id='.$id.'#'.$nomeAluno.'#'.$acertou.'#'.$data_nascimento;
        }
    }
    else{
        echo 0;
    }
?>