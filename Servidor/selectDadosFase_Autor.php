<?php
    include("conexao.php");

    $pergunta = $_GET['id_fase'];

    $select = "SELECT f.id, f.nome as nome_fase, u.nome as nome_autor, u.data_nascimento, u.email FROM fase f LEFT JOIN usuario u ON u.id = f.autor WHERE f.id =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nome_fase = $pergunta['nome_fase'];
            $nome_autor = $pergunta['nome_autor'];
            $data_nascimento = $pergunta['data_nascimento'];
            $email = $pergunta['email'];

            echo '&id='.$id.'#'.$nome_fase.'#'.$nome_autor.'#'.$data_nascimento.'#'.$email;
        }
    }
    else{
        echo 0;
    }
?>