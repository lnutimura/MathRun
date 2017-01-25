<?php
    include("conexao.php");

    $x = $_GET['x'];
    $y = $_GET['y'];

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
?>