<?php
    include("conexao.php");

    $nome = $_GET['nome'];
    $autor = $_GET['autor'];
    $data1 = $_GET['data'];

    //$originalDate = "23-10-1995";
    $data = date("y-m-d", strtotime($data1));

    $select = "INSERT INTO fase (nome, autor, data) VALUES (:nome, :autor, :data)";

    $result = $conexao->prepare($select);
    $result->bindParam(':nome', $nome, PDO::PARAM_STR); 
    $result->bindParam(':autor', $autor, PDO::PARAM_STR); 
    $result->bindParam(':data', $data, PDO::PARAM_STR);
    $result->execute();
    
    if ($result) {
        echo 1;
    }else{
        echo 0;
    }
?>