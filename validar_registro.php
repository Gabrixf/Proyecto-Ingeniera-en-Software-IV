<?php
session_start();
require_once 'conexion.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $nombre = trim($_POST['nombre']);
    $apellido = trim($_POST['apellido']);
    $cedula = trim($_POST['cedula']);
    $telefono = trim($_POST['telefono']);
    $correo = trim($_POST['correo']);
    $contrasena = trim($_POST['contrasena']);
    $ingreso = floatval($_POST['ingreso']);

    try {
        $conn = Conexion::ConexionBD();
        $conn->beginTransaction();

        // Verificar si el correo ya existe
        $verificar = $conn->prepare("SELECT * FROM Usuarios WHERE Correo = ?");
        $verificar->execute([$correo]);
        if ($verificar->fetch()) {
            $_SESSION['mensaje'] = "El correo ya está registrado.";
            $_SESSION['tipo'] = "danger";
            header('Location: registro_cliente.php');
            exit();
        }

        $stmtUsuario = $conn->prepare("DECLARE @NewId INT;
EXEC @NewId = sp_AgregarUsuario @Nombre = ?, @Apellido = ?, @Correo = ?, @Contrasena = ?, @Rol = ?;
SELECT @NewId AS IdUsuario;");
        $rol = 'Cliente';
        $stmtUsuario->execute([$nombre, $apellido, $correo, $contrasena, $rol]);

        $idUsuario = $stmtUsuario->fetch(PDO::FETCH_ASSOC)['IdUsuario'];

        // Insertar en Clientes
        $stmtCliente = $conn->prepare("EXEC sp_AgregarCliente @IdUsuario = ?, @Nombre = ?, @Apellido = ?, @Cedula = ?, @Telefono = ?, @IngresoMensual = ?");
        $stmtCliente->execute([
            $idUsuario,
            $nombre,
            $apellido,
            $cedula,
            $telefono,
            $ingreso
        ]);

        $conn->commit();

        $_SESSION['mensaje'] = "Registro exitoso. ¡Ya podés iniciar sesión!";
        $_SESSION['tipo'] = "success";
        header('Location: login.php');
        exit();
    } catch (PDOException $e) {
        $conn->rollBack();
        $_SESSION['mensaje'] = "Error en el registro: " . $e->getMessage();
        $_SESSION['tipo'] = "danger";
        header('Location: registro_cliente.php');
        exit();
    }
} else {
    header('Location: index.php');
    exit();
}
