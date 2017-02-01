<?php
    include("conexao.php");

    $x = $_GET['x'];
    $y = $_GET['y'];
    $ordem = $_GET['ordem']; 
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
        $select = "INSERT INTO casas (id_fase, x, y, ordem, id_pergunta) VALUES (:fase, :x, :y, :ordem, :pergunta)";
        $result = $conexao->prepare($select);
        $result->bindParam(':fase', $fase, PDO::PARAM_STR); 
        $result->bindParam(':x', $x, PDO::PARAM_STR);
        $result->bindParam(':y', $y, PDO::PARAM_STR);
        $result->bindParam(':ordem', $ordem, PDO::PARAM_STR);
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
?>