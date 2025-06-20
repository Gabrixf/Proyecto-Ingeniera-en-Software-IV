<?php
session_start();
require_once '../conexion.php'; // Ruta correcta

if (!isset($_SESSION['idUsuario'])) {
    header("Location: ../login.php");
    exit();
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $monto = floatval($_POST['monto']);
    $plazo = intval($_POST['plazo']);
    $idUsuario = $_SESSION['idUsuario'];
    $saldoPendiente = $monto; // Saldo inicial = monto solicitado

    try {
        $conn = Conexion::ConexionBD();

        // 1. Buscar el Cliente por IdUsuario
        $stmtCliente = $conn->prepare("SELECT IdCliente FROM Clientes WHERE IdUsuario = ?");
        $stmtCliente->execute([$idUsuario]);
        $cliente = $stmtCliente->fetch(PDO::FETCH_ASSOC);

        if (!$cliente) {
            $_SESSION['mensaje'] = "Cliente no encontrado.";
            $_SESSION['tipo'] = "danger";
            header('Location: solicitar_prestamo.php');
            exit();
        }

        $idCliente = $cliente['IdCliente'];

        // 2. Insertar el préstamo pendiente
        $stmtPrestamo = $conn->prepare("INSERT INTO Prestamos (IdCliente, MontoPrestamo, SaldoPendiente, PlazoMeses, Estado)
                                        VALUES (?, ?, ?, ?, 'Pendiente')");
        $stmtPrestamo->execute([$idCliente, $monto, $saldoPendiente, $plazo]);

        $_SESSION['mensaje'] = "¡Solicitud enviada! Ahora está pendiente de aprobación.";
        $_SESSION['tipo'] = "success";
        header('Location: solicitar_prestamo.php');
        exit();

    } catch (PDOException $e) {
        $_SESSION['mensaje'] = "Error al enviar la solicitud: " . $e->getMessage();
        $_SESSION['tipo'] = "danger";
        header('Location: solicitar_prestamo.php');
        exit();
    }
} else {
    header('Location: solicitar_prestamo.php');
    exit();
}
?>