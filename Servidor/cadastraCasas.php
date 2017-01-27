<?php
    include("conexao.php");

    $x = $_GET['x'];
    $y = $_GET['y'];
    $pergunta = $_GET['pergunta']; 
    $nome_fase = $_GET['fase'];
    $autor = $_GET['autor']; 


    //pega a ultima fase cadastrada do autor
    $select1 = "SELECT id FROM fase WHERE nome = :nome_fase AND autor = :autor ORDER BY id DESC LIMIT 1";
    $resultado = $conexao->prepare($select1);
    $resultado->bindParam(':nome_fase', $nome_fase, PDO::PARAM_STR);
    $resultado->bindParam(':autor', $autor, PDO::PARAM_STR);
    $resultado->execute();
    $count = $resultado->rowCount(); 
    //echo 1;

    //if ($count > 0) {
        //echo 2;
        
        foreach($resultado as $res1) {
            $fase = $res1['id'];
        }
        //echo $fase;
        
        //cadastra a casa
        $select = "INSERT INTO casas (id_fase, x, y, id_pergunta) VALUES (:fase, :x, :y, :pergunta)";
        $result = $conexao->prepare($select);
        $result->bindParam(':fase', $fase, PDO::PARAM_STR); 
        $result->bindParam(':x', $x, PDO::PARAM_STR);
        $result->bindParam(':y', $y, PDO::PARAM_STR);
        $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR);
        $result->execute();
        if ($result) {
            echo 1;
        }else{
            echo 0;
        } 
        
    //}else{
    //    $echo 0;
    //}

/*
    //pega a ultima fase cadastrada
    $select1 = "SELECT id FROM fase ORDER BY id DESC LIMIT 1";
    $result1 = $conexao->prepare($select1);
    $result1->execute();

    foreach($result1 as $res1) {
        $fase = $res1['id'];
    }

    //pega a ultima pergunta cadastrada
    $select2 = "SELECT id FROM perguntas ORDER BY id DESC LIMIT 1";
    $result2 = $conexao->prepare($select2);
    $result2->execute();

    foreach($result2 as $res2) {
        $pergunta = $res2['id'];
    }

    $select = "INSERT INTO casas (id_fase, x, y, id_pergunta) VALUES (:fase, :x, :y, :pergunta)";

    $result = $conexao->prepare($select);
    $result->bindParam(':fase', $fase, PDO::PARAM_STR); 
    $result->bindParam(':x', $x, PDO::PARAM_STR);
    $result->bindParam(':y', $y, PDO::PARAM_STR);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR);
    $result->execute();
    if ($result) {
        echo 1;
    }else{
        echo 0;
    }
*/
?>