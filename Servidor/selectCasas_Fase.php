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
            $id = $casas['id'];
            $x = $casas['x'];
            $y = $casas['y'];
            $pergunta = $casas['id_pergunta'];

            echo '&id='.$id.'#'.$x.'#'.$y.'#'.$pergunta;
        }
    }
    else{
        echo 0;
    }
?>