<?php
    include("conexao.php");

    $login = $_GET['login'];
    $senha = $_GET['senha'];

    $select = "SELECT * FROM usuario WHERE login=:login AND senha=:senha";

    $result = $conexao->prepare($select);
    $result->bindParam(':login', $login, PDO::PARAM_STR); 
    $result->bindParam(':senha', $senha, PDO::PARAM_STR);
    $result->execute();
    $count = $result->rowCount(); 
    
    if($count==1) {
        echo 1;
        foreach($result as $user) {
	        $id = $user['id'];
	        $is_prof = $user['is_prof'];
	        echo '&id='.$id.'#'.$is_prof;
	    }
    }
    else{
        echo 0;
    }
?>