<?php
session_start();
require_once '../conexion.php';

// Validar que sea un Admin
if (!isset($_SESSION['idUsuario']) || $_SESSION['rol'] !== 'Admin') {
    header('Location: ../login.php');
    exit();
}

// Validar que vengan los datos
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['idPrestamo'], $_POST['accion'])) {
    $idPrestamo = intval($_POST['idPrestamo']);
    $accion = $_POST['accion'];

    try {
        $conn = Conexion::ConexionBD();

        // Aceptar o Rechazar el préstamo
        if ($accion === 'aceptar') {
            $stmt = $conn->prepare("UPDATE Prestamos SET Estado = 'Aprobado' WHERE IdPrestamo = ?");
        } elseif ($accion === 'rechazar') {
            $stmt = $conn->prepare("UPDATE Prestamos SET Estado = 'Rechazado' WHERE IdPrestamo = ?");
        } else {
            $_SESSION['mensaje'] = 'Acción inválida.';
            $_SESSION['tipo'] = 'danger';
            header('Location: ver_solicitudes.php');
            exit();
        }

        $stmt->execute([$idPrestamo]);

        $_SESSION['mensaje'] = 'Solicitud actualizada correctamente.';
        $_SESSION['tipo'] = 'success';
        header('Location: ver_solicitudes.php');
        exit();

    } catch (PDOException $e) {
        $_SESSION['mensaje'] = 'Error al actualizar el préstamo: ' . $e->getMessage();
        $_SESSION['tipo'] = 'danger';
        header('Location: ver_solicitudes.php');
        exit();
    }
} else {
    $_SESSION['mensaje'] = 'Solicitud inválida.';
    $_SESSION['tipo'] = 'danger';
    header('Location: ver_solicitudes.php');
    exit();
}
?>