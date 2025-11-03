/**
 * Get evaluation data from the form and selected schools
 * @returns {Object} JSON object with plan details and selected schools
 */
function getEvaluationData() {
    // Get plan name from the title input
    const planName = document.getElementById('username')?.value || '';
    
    // Get plan type from the select dropdown
    const planTypeSelect = document.getElementById('userRole');
    const planType = planTypeSelect?.value || '';
    
    // Get date range and parse start/end dates
    const dateRangeInput = document.getElementById('dateRange');
    let startDate = '';
    let endDate = '';
    
    if (dateRangeInput && dateRangeInput.value) {
        // Parse the date range (format depends on your date picker)
        // Assuming format like "DD/MM/YYYY to DD/MM/YYYY" or "DD/MM/YYYY - DD/MM/YYYY"
        const dateValue = dateRangeInput.value;
        const dates = dateValue.split(/\s*(?:to|-)\s*/);
        
        if (dates.length === 2) {
            startDate = dates[0].trim();
            endDate = dates[1].trim();
        } else if (dates.length === 1) {
            // If only one date is provided, use it as both start and end
            startDate = dates[0].trim();
            endDate = dates[0].trim();
        }
    }
    
    // Get all selected schools from the table
    const selectedSchools = [];
    const tableRows = document.querySelectorAll('#userTable tbody tr');
    
    tableRows.forEach((row, index) => {
        const checkbox = row.querySelector('.row-select');
        
        if (checkbox && checkbox.checked) {
            // Get school ID from data attribute (you'll need to add this to your HTML)
            // If not present, use the row index as a fallback
            const schoolId = row.getAttribute('data-school-id') || String(index + 1);
            
            // Get visit type from the select dropdown in the row
            // Try multiple selectors to handle different HTML structures
            let visitTypeSelect = row.querySelector('.visit-type-select') || 
                                 row.querySelector('select.form-select') ||
                                 row.querySelector('select');
            const visitType = visitTypeSelect?.value || '';
            
            // Get the school name if available
            const schoolNameElement = row.querySelector('h6');
            const schoolName = schoolNameElement?.textContent?.trim() || '';
            
            selectedSchools.push({
                SchoolId: schoolId,
                VisitType: visitType,
                SchoolName: schoolName // Optional: include school name for reference
            });
        }
    });
    
    // Return the formatted JSON object
    return {
        PlanName: planName,
        PlanType: planType,
        StartDate: startDate,
        EndDate: endDate,
        SelectedSchool: selectedSchools
    };
}

/**
 * Validate evaluation data before submission
 * @param {Object} data - The evaluation data object
 * @returns {Object} - {isValid: boolean, errors: string[]}
 */
function validateEvaluationData(data) {
    const errors = [];
    
    if (!data.PlanName || data.PlanName.trim() === '') {
        errors.push('الرجاء إدخال عنوان الخطة');
    }
    
    if (!data.PlanType || data.PlanType === '') {
        errors.push('الرجاء اختيار نوع الخطة');
    }
    
    if (!data.StartDate || data.StartDate === '') {
        errors.push('الرجاء اختيار تاريخ البداية');
    }
    
    if (!data.EndDate || data.EndDate === '') {
        errors.push('الرجاء اختيار تاريخ النهاية');
    }
    
    if (data.SelectedSchool.length === 0) {
        errors.push('الرجاء تحديد مدرسة واحدة على الأقل');
    }
    
    // Check if any selected school is missing visit type
    const schoolsWithoutVisitType = data.SelectedSchool.filter(
        school => !school.VisitType || school.VisitType === ''
    );
    
    if (schoolsWithoutVisitType.length > 0) {
        errors.push(`الرجاء اختيار نوع الزيارة لجميع المدارس المحددة (${schoolsWithoutVisitType.length} مدرسة بدون نوع زيارة)`);
    }
    
    return {
        isValid: errors.length === 0,
        errors: errors
    };
}

/**
 * Example usage with console output
 */
function testGetEvaluationData() {
    const data = getEvaluationData();
    console.log('Evaluation Data:', JSON.stringify(data, null, 2));
    
    const validation = validateEvaluationData(data);
    console.log('Validation Result:', validation);
    
    return data;
}

// Example of how to use the function when a button is clicked
document.addEventListener('DOMContentLoaded', function() {
    // You can attach this to your save button
    const saveButton = document.querySelector('button[type="submit"]');
    
    if (saveButton) {
        saveButton.addEventListener('click', function(e) {
            e.preventDefault(); // Prevent form submission
            
            const evaluationData = getEvaluationData();
            console.log('Collected Evaluation Data:', evaluationData);
            
            // Validate the data
            const validation = validateEvaluationData(evaluationData);
            
            if (!validation.isValid) {
                // Show errors to user
                alert('يرجى تصحيح الأخطاء التالية:\n\n' + validation.errors.join('\n'));
                return;
            }
            
            // Data is valid, proceed with submission
            console.log('Data is valid! Ready to submit.');
            
            // You can now send this data to your server
            // Example: sendToServer(evaluationData);
            
            // Or show confirmation modal
            showConfirmationModal(evaluationData);
        });
    }
});

/**
 * Show confirmation modal with the collected data
 */
function showConfirmationModal(data) {
    const modal = document.getElementById('confirmation-modal');
    if (modal) {
        const modalText = modal.querySelector('h5');
        if (modalText) {
            modalText.textContent = `تم تحديد (${data.SelectedSchool.length.toString().padStart(2, '0')}) مدرسة للإضافة للخطة`;
        }
        
        // Use Bootstrap's modal API
        const bootstrapModal = new bootstrap.Modal(modal);
        bootstrapModal.show();
        
        // Handle confirmation button
        const confirmButton = modal.querySelector('.btn-primary');
        if (confirmButton) {
            confirmButton.onclick = function() {
                sendToServer(data);
                bootstrapModal.hide();
            };
        }
    }
}

/**
 * Send data to server (example implementation)
 */
function sendToServer(data) {
    console.log('Sending to server:', data);
    
    // Example API call
    /*
    fetch('/api/evaluation-plans', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data)
    })
    .then(response => response.json())
    .then(result => {
        console.log('Success:', result);
        alert('تم حفظ البيانات بنجاح');
        // Redirect or update UI
    })
    .catch(error => {
        console.error('Error:', error);
        alert('حدث خطأ أثناء حفظ البيانات');
    });
    */
}
