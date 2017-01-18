<?php
    include("conexao.php");

    $pergunta = $_GET['pergunta'];

    $select = "SELECT * FROM perguntas WHERE id=:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $questao = $pergunta['questao'];
            $resposta = $pergunta['resposta'];
            $dificuldade = $pergunta['dificuldade'];
            $tipo = $pergunta['tipo'];
            $autor = $pergunta['autor'];

            echo '&id='.$id.'#'.$questao.'#'.$resposta.'#'.$dificuldade.'#'.$tipo.'#'.$autor;
        }
    }
    else{
        echo 0;
    }
?>