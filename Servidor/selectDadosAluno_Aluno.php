<?php
    include("conexao.php");

    $pergunta = $_GET['id_usuario'];

    $select = "SELECT u.id, u.nome, u.email, u.data_nascimento
                FROM usuario u
                WHERE u.id =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nome = $pergunta['nome'];
            $data_nascimento = $pergunta['data_nascimento'];
            $email = $pergunta['email'];

            echo '&id='.$id.'#'.$nome.'#'.$data_nascimento.'#'.$email;
        }
    }
    else{
        echo 0;
    }
?>