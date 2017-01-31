<?php
    include("conexao.php");

    $usuario = $_GET['usuario'];
    $fase = $_GET['fase'];
    $pergunta = $_GET['pergunta'];
    $resposta = $_GET['resposta'];	//usar 2.5 e nao 2,5
    $acertou = $_GET['acertou'];
    $data1 = $_GET['data'];

    //$originalDate = "23-10-1995";
    $data = date("y-m-d", strtotime($data1));

    $select = 
        "INSERT INTO historico (id_usuario, id_fase, id_pergunta, resposta_dada, acertou, data) VALUES (:usuario, :fase, :pergunta, :resposta, :acertou, :data)";

    $result = $conexao->prepare($select);
    $result->bindParam(':usuario', $usuario, PDO::PARAM_STR); 
    $result->bindParam(':fase', $fase, PDO::PARAM_STR);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR);
    $result->bindParam(':resposta', $resposta, PDO::PARAM_STR);
    $result->bindParam(':acertou', $acertou, PDO::PARAM_STR);
    $result->bindParam(':data', $data, PDO::PARAM_STR);
    $result->execute();

    if ($result) {
        echo 1;
    }else{
        echo 0;
    }
?>
