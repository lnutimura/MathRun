<?php
    include("conexao.php");

    $select = "SELECT * FROM usuario WHERE is_prof = 0 ORDER BY id";
    
    $result = $conexao->prepare($select);
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $fases) {
            $id = $fases['id'];
            $nome = $fases['nome'];
            $email = $fases['email'];

            echo '&id='.$id.'#'.$nome.'#'.$email;
        }
    }
    else{
        echo 0;
    }
?>