<?php
    include("conexao.php");

    $email = $_GET['email'];
    $nome = $_GET['nome'];
    $login = $_GET['login'];
    $senha = $_GET['senha'];
    $data1 = $_GET['data'];
    $prof = $_GET['prof'];
    $isprof = $_GET['isprof'];

    //$originalDate = "23-10-1995";
    $data = date("y-m-d", strtotime($data1));

    $select = "INSERT INTO usuario (email, nome, login, senha, data_nascimento, id_prof, is_prof) VALUES (:email, :nome, :login, :senha, :data, :prof, :isprof)";

    $result = $conexao->prepare($select);
    $result->bindParam(':email', $email, PDO::PARAM_STR); 
    $result->bindParam(':nome', $nome, PDO::PARAM_STR);
    $result->bindParam(':login', $login, PDO::PARAM_STR);
    $result->bindParam(':senha', $senha, PDO::PARAM_STR);
    $result->bindParam(':data', $data, PDO::PARAM_STR);
    $result->bindParam(':prof', $prof, PDO::PARAM_STR);
    $result->bindParam(':isprof', $isprof, PDO::PARAM_STR);
    $result->execute();
    
    if ($result) {
        echo 1;
    }else{
        echo 0;
    }
?>