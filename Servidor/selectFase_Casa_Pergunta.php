<?php
    include("conexao.php");

    $fase = $_GET['fase'];

    $select = "SELECT * FROM casas WHERE id_fase=:fase ORDER BY id";

    $result = $conexao->prepare($select);
    $result->bindParam(':fase', $fase, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $casas) {
            $id_casa = $casas['id'];
            $x = $casas['x'];
            $y = $casas['y'];
            $id_pergunta = $casas['id_pergunta'];

            $select1 = "SELECT * FROM perguntas WHERE id=:id_pergunta";

            $result1 = $conexao->prepare($select1);
            $result1->bindParam(':id_pergunta', $id_pergunta, PDO::PARAM_STR); 
            $result1->execute();
            $count1 = $result1->rowCount(); 

            if($count1 > 0) {
                foreach($result1 as $pergunta) {
                    $questao = $pergunta['questao'];
                    $resposta = $pergunta['resposta'];
                    $dificuldade = $pergunta['dificuldade'];
                    $tipo = $pergunta['tipo'];
                    $autor = $pergunta['autor'];

                    echo '&id='.$id_casa.'#'.$x.'#'.$y.'#'.$id_pergunta.'#'.$questao.'#'.$resposta.'#'.$dificuldade.'#'.$tipo.'#'.$autor;
                }
            }
            else{
                echo 0;
            }
        }
    }
    else{
        echo 0;
    }
?>