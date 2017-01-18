<?php
    include("conexao.php");

    $usuario = $_GET['usuario'];

    $select = "SELECT * FROM historico WHERE id_usuario=:usuario ORDER BY id";

    $result = $conexao->prepare($select);
    $result->bindParam(':usuario', $usuario, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id_fase = $pergunta['id_fase'];
            $id_pergunta = $pergunta['id_pergunta'];
            $resposta_dada = $pergunta['resposta_dada'];
            $acertou = $pergunta['acertou'];
            $data = $pergunta['data'];

            echo '&id='.$id_fase.'#'.$id_pergunta.'#'.$resposta_dada.'#'.$acertou.'#'.$data;
        }
    }
    else{
        echo 0;
    }
?>