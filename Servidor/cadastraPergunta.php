<?php
    include("conexao.php");

    $questao = $_GET['questao']; 			//string 200
    $resposta = $_GET['resposta']; 			//float
    $dificuldade = $_GET['dificuldade']; 	//int
    $tipo = $_GET['tipo']; 					//int
    $autor = $_GET['autor']; 				//int FK usuario (id)

    $select = "INSERT INTO perguntas (questao, resposta, dificuldade, tipo, autor) VALUES (:questao, :resposta, :dificuldade, :tipo, :autor)";

    $result = $conexao->prepare($select);
    $result->bindParam(':questao', $questao, PDO::PARAM_STR); 
    $result->bindParam(':resposta', $resposta, PDO::PARAM_STR);
    $result->bindParam(':dificuldade', $dificuldade, PDO::PARAM_STR);
    $result->bindParam(':tipo', $tipo, PDO::PARAM_STR);
    $result->bindParam(':autor', $autor, PDO::PARAM_STR);
    $result->execute();
    if ($result) {
        echo 1;
    }else{
        echo 0;
    }
?>