<?php
    include("conexao.php");

    $select = "SELECT * FROM perguntas ORDER BY id DESC LIMIT 1";
    
    $result = $conexao->prepare($select);
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $perguntas) {
            $id = $perguntas['id'];
            $questao = $perguntas['questao'];
            $resposta = $perguntas['resposta'];
            $dificuldade = $perguntas['dificuldade'];
            $tipo = $perguntas['tipo'];
            $autor = $perguntas['autor'];

            echo '&id='.$id.'#'.$questao.'#'.$resposta.'#'.$dificuldade.'#'.$tipo.'#'.$autor;
        }
    }
    else{
        echo 0;
    }
?>