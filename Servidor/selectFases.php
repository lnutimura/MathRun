<?php
    include("conexao.php");

    $select = "SELECT * FROM fase ORDER BY id";
    
    $result = $conexao->prepare($select);
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $fases) {
            $id = $fases['id'];
            $nome = $fases['nome'];
            $autor = $fases['autor'];
            $data = $fases['data'];

            echo '&id='.$id.'#'.$nome.'#'.$autor.'#'.$data;
        }
    }
    else{
        echo 0;
    }
?>