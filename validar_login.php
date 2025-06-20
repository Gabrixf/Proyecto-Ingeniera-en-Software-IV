<?php
ob_start(); // Para evitar errores de headers
session_start();
require_once 'conexion.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $correo = $_POST['correo'];
    $contrasena = $_POST['contrasena'];

    try {
        $conn = Conexion::ConexionBD();

        $stmt = $conn->prepare("EXEC sp_LoginUsuario @correo = ?, @contrasena = ?");
        $stmt->execute([$correo, $contrasena]);
        $usuario = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($usuario) {
            $_SESSION['idUsuario'] = $usuario['IdUsuario'];
            $_SESSION['nombre'] = $usuario['Nombre'];
            $_SESSION['rol'] = $usuario['Rol'];

            // Redireccionar dependiendo del rol
            if ($usuario['Rol'] === 'Cliente') {
                header("Location: Cliente/dashboard.php");
                exit();
            } elseif ($usuario['Rol'] === 'Admin') {
                header("Location: Admin/dashboard.php");
                exit();
            } else {
                // Si por alguna razón no es Cliente ni Admin
                $_SESSION['mensaje'] = "Rol no reconocido.";
                header("Location: login.php");
                exit();
            }
        } else {
            $_SESSION['mensaje'] = "Credenciales incorrectas.";
            header("Location: login.php");
            exit();
        }
    } catch (PDOException $e) {
        $_SESSION['mensaje'] = "Error de conexión: " . $e->getMessage();
        header("Location: login.php");
        exit();
    }
}
?><?php
ob_start(); // Para evitar errores de headers
session_start();
require_once 'conexion.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $correo = $_POST['correo'];
    $contrasena = $_POST['contrasena'];

    try {
        $conn = Conexion::ConexionBD();

        $stmt = $conn->prepare("EXEC sp_LoginUsuario @correo = ?, @contrasena = ?");
        $stmt->execute([$correo, $contrasena]);
        $usuario = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($usuario) {
            $_SESSION['idUsuario'] = $usuario['IdUsuario'];
            $_SESSION['nombre'] = $usuario['Nombre'];
            $_SESSION['rol'] = $usuario['Rol'];

            // Redireccionar dependiendo del rol
            if ($usuario['Rol'] === 'Cliente') {
                header("Location: Cliente/dashboard.php");
                exit();
            } elseif ($usuario['Rol'] === 'Admin') {
                header("Location: Admin/dashboard.php");
                exit();
            } else {
                // Si por alguna razón no es Cliente ni Admin
                $_SESSION['mensaje'] = "Rol no reconocido.";
                header("Location: login.php");
                exit();
            }
        } else {
            $_SESSION['mensaje'] = "Credenciales incorrectas.";
            header("Location: login.php");
            exit();
        }
    } catch (PDOException $e) {
        $_SESSION['mensaje'] = "Error de conexión: " . $e->getMessage();
        header("Location: login.php");
        exit();
    }
}
?>